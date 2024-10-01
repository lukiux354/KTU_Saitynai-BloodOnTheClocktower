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
            var commentsGroup = app.MapGroup("/api/scripts/{scriptId}/characters/{characterId}/comments").AddFluentValidationAutoValidation();

            // GET all comments for a specific character
            commentsGroup.MapGet("/", async (int scriptId, int characterId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                // Get the comments by joining through the Character entity
                var comments = await dbContext.Comments
                    .Where(comment => comment.Character.Id == characterId && comment.Character.Script.Id == scriptId)
                    .ToListAsync(cancellationToken);

                // Check if no comments are found
                if (comments.Count == 0)
                {
                    return Results.NotFound("No comments found for this character.");
                }

                // Return the list of comments in DTO form
                return TypedResults.Ok(comments.Select(comment => comment.ToDto()));
            });

            // GET a specific comment by ID
            commentsGroup.MapGet("/{commentId}", async (int scriptId, int characterId, int commentId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok(comment.ToDto());
            });

            // POST a new comment for a character
            commentsGroup.MapPost("/", async (int scriptId, int characterId, CreateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var character = await dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.Script.Id == scriptId, cancellationToken);
                if (character == null)
                {
                    return Results.NotFound();
                }

                var comment = new Comment
                {
                    Content = dto.Content,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Character = character
                };

                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Created($"/api/scripts/{scriptId}/characters/{characterId}/comments/{comment.Id}", comment.ToDto());
            });

            // PUT (update) an existing comment
            commentsGroup.MapPut("/{commentId}", async (int scriptId, int characterId, int commentId, UpdateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null)
                {
                    return Results.NotFound();
                }

                comment.Content = dto.Content;
                dbContext.Comments.Update(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Ok(comment.ToDto());
            });

            // DELETE a comment by ID
            commentsGroup.MapDelete("/{commentId}", async (int scriptId, int characterId, int commentId, ForumDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null)
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
