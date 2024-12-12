using Saitynai.Data.Entities;
using Saitynai.Data;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Saitynai.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Saitynai
{
    public static class ScriptEndpoints
    {


        public static void AddScriptApi(this WebApplication app)
        {
            var scriptsGroups = app.MapGroup("/api").AddFluentValidationAutoValidation();

            scriptsGroups.MapGet("/scripts", async (ForumDbContext dbContext) =>
            {
                return (await dbContext.Scripts.ToListAsync()).Select(script => script.ToDto());
            });

            scriptsGroups.MapGet("/scripts/{scriptId:int}", async (int scriptId, ForumDbContext dbContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                return script == null ? Results.NotFound() : TypedResults.Ok(script.ToDto());
            });

            scriptsGroups.MapPost("/scripts", [Authorize(Roles = ForumRoles.ForumUser)] async (CreateScriptDto dto, ForumDbContext dbContext, HttpContext httpContext) =>
            {
                var script = new Script { Title = dto.Title, Description = dto.Description, CreatedAt = DateTimeOffset.UtcNow, UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)};
                dbContext.Scripts.Add(script);

                await dbContext.SaveChangesAsync();

                return TypedResults.Created($"api/script/{script.Id}", script.ToDto());
            });

            scriptsGroups.MapPut("/scripts/{scriptId:int}", [Authorize]async (UpdateScriptDto dto, int scriptId, ForumDbContext dbContext, HttpContext httpContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return Results.NotFound();
                }

                if(!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != script.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                script.Description = dto.Description;

                dbContext.Scripts.Update(script);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(script.ToDto());
            });

            scriptsGroups.MapDelete("/scripts/{scriptId:int}", [Authorize]async (int scriptId, ForumDbContext dbContext, HttpContext httpContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return Results.NotFound();
                }

                if (!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != script.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                dbContext.Scripts.Remove(script);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });
        }
    }
}
