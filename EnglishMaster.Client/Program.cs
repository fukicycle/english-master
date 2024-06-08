using Blazored.LocalStorage;
using EnglishMaster.Client;
using EnglishMaster.Client.Authentication;
using EnglishMaster.Client.Services;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using Fukicycle.Tool.AppBase;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(nameof(ApplicationMode.Dev), httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:5211");
});
builder.Services.AddHttpClient(nameof(ApplicationMode.Prod), httpClient =>
{
    httpClient.BaseAddress = new Uri("https://www.sato-home.mydns.jp:9445");
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(ApplicationMode.Dev)));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSpeechSynthesis();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<DictionaryClientService>();
builder.Services.AddScoped<AchivementClientService>();
builder.Services.AddScoped<ResultClientService>();
builder.Services.AddScoped<ProgressClientService>();
builder.Services.AddScoped<QuestionClientService>();
builder.Services.AddScoped<SpeakService>();
builder.Services.AddScoped<PartOfSpeechClientService>();
builder.Services.AddScoped<LevelClientService>();
builder.Services.AddScoped<UserRegisterService>();
builder.Services.AddScoped<LineChartClientService>();
builder.Services.AddScoped<RadarChartClientService>();
builder.Services.AddScoped<TreeFarmService>();
builder.Services.AddScoped<OAuthConfigurationService>();
builder.Services.AddAppBase();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(service => service.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

//builder.Services.AddOidcAuthentication(options =>
//{
//    builder.Configuration.Bind("Google", options.ProviderOptions);
//});

await builder.Build().RunAsync();
