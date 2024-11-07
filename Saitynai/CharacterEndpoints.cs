﻿using Saitynai.Data.Entities;
using Saitynai.Data;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Microsoft.AspNetCore.Http;
using Saitynai.Auth.Model;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Saitynai
{
    public static class CharacterEndpoints
    {
        public static void AddCharacterApi(this WebApplication app)
        {
            var charactersGroup = app.MapGroup("/api/scripts/{scriptId:int}").AddFluentValidationAutoValidation();


            charactersGroup.MapGet("characters", async (int scriptId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                return (await dbContext.Characters.ToListAsync(cancellationToken)).Select(script => script.ToDto());
            });


            charactersGroup.MapGet("/characters/{characterId:int}", async (int scriptId, int characterId, ForumDbContext dbContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                return character == null ? Results.NotFound() : Results.Ok(character.ToDto());
            });

            charactersGroup.MapPost("/characters/", [Authorize(Roles = ForumRoles.ForumUser)] async (int scriptId, CreateCharacterDto dto, ForumDbContext dbContext, HttpContext httpContext) =>
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

                var character = new Character
                {
                    Title = dto.Title,
                    Body = dto.Body,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Script = script,
                    UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                };
                dbContext.Characters.Add(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.Created($"/api/scripts/{scriptId}/characters/{character.Id}", character.ToDto());
            });

            charactersGroup.MapPut("/characters/{characterId:int}", [Authorize] async (int scriptId, int characterId, UpdateCharacterDto dto, ForumDbContext dbContext, HttpContext httpContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                if (character == null)
                {
                    return Results.NotFound();
                }

                if (!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != character.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                character.Body = dto.Body;

                dbContext.Characters.Update(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(character.ToDto());
            });

            charactersGroup.MapDelete("/characters/{characterId:int}", [Authorize] async (int scriptId, int characterId, ForumDbContext dbContext, HttpContext httpContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                if (character == null)
                {
                    return Results.NotFound();
                }
                if (!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != character.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                dbContext.Characters.Remove(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });

        }
    }
}
