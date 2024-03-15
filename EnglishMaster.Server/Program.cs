using EnglishMaster.Server;
using EnglishMaster.Server.Services;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        policy.WithOrigins("https://localhost:7132", "https://fukicycle.github.io")
                .WithMethods("GET", "POST", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type")
                .AllowCredentials();
    });
});

//Jwt authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "sato-home.mydns.jp",
        ValidAudience = "fukicycle.github.io",
        IssuerSigningKey = new SymmetricSecurityKey(ApplicationSettings.JWT_KEY),
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

//Register service
builder.Services.AddDbContext<DB>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IPartOfSpeechService, PartOfSpeechService>();
builder.Services.AddScoped<ILevelService, LevelService>();

var app = builder.Build();

//CORS
app.UseCors();
//JWT
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
