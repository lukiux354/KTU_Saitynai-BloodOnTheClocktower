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

            commentsGroup.MapPost("/", [Authorize(Roles = ForumRoles.ForumUser)] async (int scriptId, int characterId, CreateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken, HttpContext httpContext) =>
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
                    UserId = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                };

                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Created($"/api/scripts/{scriptId}/characters/{characterId}/comments/{comment.Id}", comment.ToDto());
            });

            commentsGroup.MapPut("/{commentId:int}", [Authorize] async (int scriptId, int characterId, int commentId, UpdateCommentDto dto, ForumDbContext dbContext, CancellationToken cancellationToken, HttpContext httpContext) =>
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

                if (!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != comment.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                comment.Content = dto.Content;
                dbContext.Comments.Update(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.Ok(comment.ToDto());
            });

            commentsGroup.MapDelete("/{commentId:int}", [Authorize] async (int scriptId, int characterId, int commentId, ForumDbContext dbContext, CancellationToken cancellationToken, HttpContext httpContext) =>
            {
                var comment = await dbContext.Comments
                                             .FirstOrDefaultAsync(c => c.Id == commentId && c.Character.Id == characterId && c.Character.Script.Id == scriptId, cancellationToken);

                if (comment == null || comment.Character == null || comment.Character.Script == null)
                {
                    return Results.NotFound();
                }
                if (!httpContext.User.IsInRole(ForumRoles.Admin) && httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) != comment.UserId)
                {
                    //NotFound()
                    return Results.Forbid();
                }

                dbContext.Comments.Remove(comment);
                await dbContext.SaveChangesAsync(cancellationToken);

                return TypedResults.NoContent();
            });
        }
    }
}
