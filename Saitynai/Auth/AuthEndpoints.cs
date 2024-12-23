﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Saitynai.Auth.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Saitynai.Auth
{
    public static class AuthEndpoints
    {
        public static void AddAuthApi(this WebApplication app)
        {
            //register
            app.MapPost("api/accounts", async (UserManager<ForumUser> userManager, RegisterUserDto dto) =>
            {
                //check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);
                if (user != null)
                    return Results.UnprocessableEntity("Username already taken");

                var newUser = new ForumUser()
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                };

                // TODO : padaryt kad negaletu susikurt useris be roles
                var createUserResut = await userManager.CreateAsync(newUser, dto.Password);
                if (!createUserResut.Succeeded)
                    return Results.UnprocessableEntity("createUserResult failed for some reason.");

                await userManager.AddToRoleAsync(newUser, ForumRoles.ForumUser);

                return Results.Created();
            });

            //login
            app.MapPost("api/login", async (UserManager<ForumUser> userManager, JwtTokenService jwtTokenService, SessionService sessionService, HttpContext httpContext, LoginDto dto) =>
            {
                //check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);
                if (user == null)
                    return Results.UnprocessableEntity("User doesn't exist");

                var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
                if (!isPasswordValid)
                    return Results.UnprocessableEntity("Username or password is incorrect");

                var roles = await userManager.GetRolesAsync(user);

                var sessionId = Guid.NewGuid();
                var expiresAt = DateTime.UtcNow.AddDays(3);
                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                var refreshToken = jwtTokenService.CreateRefreshToken(sessionId, user.Id, expiresAt);

                await sessionService.CreateSessionAsync(sessionId, user.Id, refreshToken, expiresAt);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expiresAt,
                    //Secure = false => should be true galimai
                };

                httpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);

                return Results.Ok(new SuccessfulLoginDto(accessToken));
            });

            //accesstoken
            app.MapPost("api/accessToken", async (UserManager<ForumUser> userManager, JwtTokenService jwtTokenService, SessionService sessionService, HttpContext httpContext) => 
            {
                if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                {
                    return Results.UnprocessableEntity();
                }

                if(!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
                {
                    return Results.UnprocessableEntity();
                }

                var sessionId = claims.FindFirstValue("SessionId");
                if(string.IsNullOrWhiteSpace(sessionId))
                {
                    return Results.UnprocessableEntity();
                }

                var sessionIdAsGuid = Guid.Parse(sessionId);
                if(!await sessionService.IsSessionValidAsync(sessionIdAsGuid, refreshToken))
                {
                    return Results.UnprocessableEntity();
                }


                var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Results.UnprocessableEntity();
                }

                var roles = await userManager.GetRolesAsync(user);

                var expiresAt = DateTime.UtcNow.AddDays(3);
                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                var newRefreshToken = jwtTokenService.CreateRefreshToken(sessionIdAsGuid, user.Id, expiresAt);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expiresAt,
                    //Secure = false => should be true galimai
                };

                httpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);

                await sessionService.ExtendSessionAsync(sessionIdAsGuid, newRefreshToken, expiresAt);

                return Results.Ok(new SuccessfulLoginDto(accessToken));

            });

            //logout
            app.MapPost("api/logout", async (UserManager<ForumUser> userManager, JwtTokenService jwtTokenService, SessionService sessionService, HttpContext httpContext) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                {
                    return Results.UnprocessableEntity();
                }

                if (!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
                {
                    return Results.UnprocessableEntity();
                }

                var sessionId = claims.FindFirstValue("SessionId");
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return Results.UnprocessableEntity();
                }

                await sessionService.InvalidateSessionAsync(Guid.Parse(sessionId));
                httpContext.Response.Cookies.Delete("RefreshToken");

                return Results.Ok();

            });

        }
        public record RegisterUserDto(string UserName, string Email, string Password);
        public record LoginDto(string UserName, string Password);
        public record SuccessfulLoginDto(string AccessToken);
    }
}
