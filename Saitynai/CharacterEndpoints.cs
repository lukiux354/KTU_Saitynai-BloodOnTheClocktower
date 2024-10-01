using Saitynai.Data.Entities;
using Saitynai.Data;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Saitynai
{
    public static class CharacterEndpoints
    {
        public static void AddCharacterApi(this WebApplication app)
        {
            var charactersGroup = app.MapGroup("/api/scripts/{scriptId}").AddFluentValidationAutoValidation();


            charactersGroup.MapGet("characters", async (int scriptId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                return (await dbContext.Characters.ToListAsync(cancellationToken)).Select(script => script.ToDto());
            });


            charactersGroup.MapGet("/characters/{characterId}", async (int scriptId, int characterId, ForumDbContext dbContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                return character == null ? Results.NotFound() : Results.Ok(character.ToDto());
            });

            charactersGroup.MapPost("/characters/", async (int scriptId, CreateCharacterDto dto, ForumDbContext dbContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return Results.NotFound();
                }

                var character = new Character
                {
                    Title = dto.Title,
                    Body = dto.Body,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Script = script
                };
                dbContext.Characters.Add(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.Created($"/api/scripts/{scriptId}/characters/{character.Id}", character.ToDto());
            });

            charactersGroup.MapPut("/characters/{characterId}", async (int scriptId, int characterId, UpdateCharacterDto dto, ForumDbContext dbContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                if (character == null)
                {
                    return Results.NotFound();
                }

                character.Body = dto.Body;

                dbContext.Characters.Update(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(character.ToDto());
            });

            charactersGroup.MapDelete("/characters/{characterId}", async (int scriptId, int characterId, ForumDbContext dbContext) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId);
                if (character == null)
                {
                    return Results.NotFound();
                }

                dbContext.Characters.Remove(character);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });

        }
    }
}
