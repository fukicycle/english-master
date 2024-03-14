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

builder.Services.AddPWAUpdater();
builder.Services.AddScoped<IStateContainer, StateContainer>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

await builder.Build().RunAsync();
