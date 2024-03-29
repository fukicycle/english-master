using Blazored.LocalStorage;
using EnglishMaster.Client;
using EnglishMaster.Client.Services;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Client.Stores;
using EnglishMaster.Shared;
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

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSpeechSynthesis();
builder.Services.AddScoped<IStateContainer, StateContainer>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Google", options.ProviderOptions);
});

await builder.Build().RunAsync();
