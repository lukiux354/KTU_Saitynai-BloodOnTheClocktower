using Saitynai.Data.Entities;
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
using System.Xml.Linq;

namespace Saitynai
{
    public static class CharacterEndpoints
    {
        public static void AddCharacterApi(this WebApplication app)
        {
            var charactersGroup = app.MapGroup("/api/scripts/{scriptId:int}").AddFluentValidationAutoValidation();

            charactersGroup.MapGet("characters", async (int scriptId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (scriptId <= 0)
                {
                    return Results.NotFound("Script ID is required and must be greater than zero.");
                }

                var characters = await dbContext.Characters
                    .Where(character => character.Script.Id == scriptId)
                    .ToListAsync(cancellationToken);

                //if (characters.Count == 0)
                //{
                //    return Results.NotFound("No Characters could be found for this Script.");
                //}
                return TypedResults.Ok(characters.Select(character => character.ToDto()));
                //return (await dbContext.Characters
                //    .Where(c => c.Script.Id == scriptId)
                //    .ToListAsync(cancellationToken))
                //    .Select(character => character.ToDto());
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
                    return Results.Forbid();
                }

                dbContext.Characters.Remove(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });
        }
    }
}
