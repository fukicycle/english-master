using EnglishMaster.Server;
using EnglishMaster.Server.Security.Authentication;
using EnglishMaster.Server.Security.Service;
using EnglishMaster.Server.Services;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

//Generate jwt key
RandomNumberGenerator.Fill(ApplicationSettings.JWT_KEY);

//Setttings Security
//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5261", "https://localhost:7132", "https://fukicycle.github.io")
                .WithMethods("GET", "POST", "OPTIONS")
                .WithHeaders("AccessToken", "Content-Type")
                .AllowCredentials();
    });
});

//Access token authentication
builder.Services.AddAuthentication(AccessTokenAuthenticationOptions.DefaultScheme)
    .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>
    (AccessTokenAuthenticationOptions.DefaultScheme, options => { });

// Add services to the container.
builder.Services.AddControllers();

//Register service
builder.Services.AddDbContext<DB>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IPartOfSpeechService, PartOfSpeechService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<ILoginService, AccessTokenLoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IAccessTokenAuthenticationService, AccessTokenAuthenticationService>();

var app = builder.Build();

//CORS
app.UseCors();
//Access token authentication.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
