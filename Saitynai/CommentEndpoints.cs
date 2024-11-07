using Saitynai.Data.Entities;
using Saitynai.Data;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Saitynai
{
    public static class CommentEndpoints
    {
        public static void AddCommentApi(this WebApplication app)
        {
            var commentsGroup = app.MapGroup("/api/scripts/{scriptId:int}/characters/{characterId:int}/comments").AddFluentValidationAutoValidation();

            commentsGroup.MapGet("/", async (int scriptId, int characterId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (scriptId <= 0)
                {
                    return Results.NotFound("Script ID is required and must be greater than zero.");
                }

                var comments = await dbContext.Comments
                    .Where(comment => comment.Character.Id == characterId && comment.Character.Script.Id == scriptId)
                    .ToListAsync(cancellationToken);

                if (comments.Count == 0)
                {
                    return Results.NotFound("No comments found for this character.");
                }

                return TypedResults.Ok(comments.Select(comment => comment.ToDto()));
            });

            commentsGroup.MapGet("/{commentId:int}", async (int scriptId, int characterId, int commentId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null || comment.Character == null || comment.Character.Script == null)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok(comment.ToDto());
            });

            commentsGroup.MapPost("/", async (int scriptId, int characterId, CreateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId, cancellationToken);
                if (character == null || character.Script == null)
                {
                    return Results.NotFound();
                }

                var comment = new Comment
                {
                    Content = dto.Content,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Character = character,
                    UserId = ""
                };

                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Created($"/api/scripts/{scriptId}/characters/{characterId}/comments/{comment.Id}", comment.ToDto());
            });

            commentsGroup.MapPut("/{commentId:int}", async (int scriptId, int characterId, int commentId, UpdateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                if (scriptId <= 0 || characterId <= 0 || commentId <= 0)
                {
                    return Results.NotFound();
                }

                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null || comment.Character == null || comment.Character.Script == null)
                {
                    return Results.NotFound();
                }

                comment.Content = dto.Content;
                dbContext.Comments.Update(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Ok(comment.ToDto());
            });

            commentsGroup.MapDelete("/{commentId:int}", async (int scriptId, int characterId, int commentId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null || comment.Character == null || comment.Character.Script == null)
                {
                    return Results.NotFound();
                }

                dbContext.Comments.Remove(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.NoContent();
            });
        }
    }
}
