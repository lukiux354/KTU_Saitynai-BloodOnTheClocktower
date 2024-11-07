/*
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet tool install --global dotnet-ef

dotnet add package FluentValidation
dotnet add package FluentValidation.DependancyInjectionExtensions
dotnet add package SharpGrip.FluentValidation.AutoValidation.Endpoints

+lab2 visi


*/

//lab1 libraries
using Microsoft.EntityFrameworkCore;
using Saitynai.Data;
using Saitynai.Data.Entities;
using FluentValidation;
using FluentValidation.Results;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;
using Saitynai;
using Saitynai.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//lab2 libraries
using Saitynai.Auth;
//

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ForumDbContext>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<ProblemDetailsResultFactory>();
});

//lab2
builder.Services.AddTransient<JwtTokenService>();
builder.Services.AddTransient<SessionService>();
builder.Services.AddScoped<AuthSeeder>();
//////////////

builder.Services.AddIdentity<ForumUser, IdentityRole>()
    .AddEntityFrameworkStores<ForumDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:ValidAudience"];
    options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:ValidIssuer"];
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));

});

builder.Services.AddAuthorization();

var app = builder.Build();

using var scope = app.Services.CreateScope();

////var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

var dbSeeder = scope.ServiceProvider.GetRequiredService<AuthSeeder>();
await dbSeeder.SeedAsync();

/*
    /api/v1/scripts GET List 200
    /api/v1/scripts POST Create 201
    /api/v1/scripts/{id} GET One 200
    /api/v1/scripts/{id} PUT/PATCH Modify 200
    /api/v1/scripts/{id} DELETE Remove 200/204
*/

app.AddAuthApi();

////////////////////////////////////// API's /////////////////////////////////////////////////
app.AddScriptApi();
app.AddCharacterApi();
app.AddCommentApi();
////////////////////////////////////////////////////////////////////////////////////////////////
app.UseAuthentication();
app.UseAuthorization();

app.Run();


public class ProblemDetailsResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
    {
        var problemDetails = new HttpValidationProblemDetails(validationResult.ToValidationProblemErrors())
        {
            Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
            Title = "Unprocessable Entity",
            Status = 422
        };

        return TypedResults.Problem(problemDetails);
    }
}



public record ScriptDto(int Id, string Title, string Description, DateTimeOffset CreatedOn);
public record CharacterDto(int Id, string Title, string Body, DateTimeOffset CreatedOn);
public record CommentDto(int Id, string Content, DateTimeOffset CreatedOn);




public record CreateScriptDto(string Title, string Description)
{
    public class CreateScriptDtoValidator : AbstractValidator<CreateScriptDto>
    {
        public CreateScriptDtoValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().Length(min:2, max:50);
            RuleFor(x => x.Description).NotEmpty().Length(min: 5, max: 300);
        }
    }
};
public record CreateCharacterDto(string Title, string Body)
{
    public class CreateCharacterDtoValidator : AbstractValidator<CreateCharacterDto>
    {
        public CreateCharacterDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(min: 2, max: 50);
            RuleFor(x => x.Body).NotEmpty().Length(min: 5, max: 300);
        }
    }
};
public record CreateCommentDto(string Content)
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content).NotEmpty().Length(min: 1, max: 200);
        }
    }
};





public record UpdateScriptDto(string Description)
{
    public class UpdateScriptDtoValidator : AbstractValidator<UpdateScriptDto>
    {
        public UpdateScriptDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty().Length(min: 5, max: 300);
        }
    }
};

public record UpdateCharacterDto(string Body)
{
    public class UpdateCharacterDtoValidator : AbstractValidator<UpdateCharacterDto>
    {
        public UpdateCharacterDtoValidator()
        {
            RuleFor(x => x.Body).NotEmpty().Length(min: 5, max: 300);
        }
    }
};

public record UpdateCommentDto(string Content)
{
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Content).NotEmpty().Length(min: 1, max: 200);
        }
    }
};