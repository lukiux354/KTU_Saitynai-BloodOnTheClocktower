using Saitynai.Data.Entities;
using Saitynai.Data;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

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

            scriptsGroups.MapPost("/scripts", async (CreateScriptDto dto, ForumDbContext dbContext) =>
            {
                var script = new Script { Title = dto.Title, Description = dto.Description, CreatedAt = DateTimeOffset.UtcNow };
                dbContext.Scripts.Add(script);

                await dbContext.SaveChangesAsync();

                return TypedResults.Created($"api/script/{script.Id}", script.ToDto());
            });

            scriptsGroups.MapPut("/scripts/{scriptId:int}", async (UpdateScriptDto dto, int scriptId, ForumDbContext dbContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return Results.NotFound();
                }

                script.Description = dto.Description;

                dbContext.Scripts.Update(script);
                await dbContext.SaveChangesAsync();

                return TypedResults.Ok(script.ToDto());
            });

            scriptsGroups.MapDelete("/scripts/{scriptId:int}", async (int scriptId, ForumDbContext dbContext) =>
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return Results.NotFound();
                }

                dbContext.Scripts.Remove(script);
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            });
        }
    }
}
