using EnglishMaster.Shared;
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
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
