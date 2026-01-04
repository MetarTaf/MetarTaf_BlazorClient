using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BlazorClient;
using BlazorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base URL - tom = samme host (produktion), udfyldt = ekstern API (udvikling)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrWhiteSpace(apiBaseUrl))
    apiBaseUrl = builder.HostEnvironment.BaseAddress;

// HttpClient til API kald
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Services
builder.Services.AddScoped<WeatherApiService>();
builder.Services.AddScoped(sp => new WeatherHubService(apiBaseUrl));

// LocalStorage til at gemme airports og acknowledgements
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
