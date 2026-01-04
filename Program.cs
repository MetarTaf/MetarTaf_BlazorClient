using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BlazorClient;
using BlazorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Bestem API-URL baseret på hostname
var hostname = builder.HostEnvironment.BaseAddress;
string apiBaseUrl;

if (hostname.Contains("pre.metartaf") || hostname.Contains("localhost"))
{
    apiBaseUrl = "https://pre.metartafapi.cbmprojects.dk";
}
else
{
    apiBaseUrl = "https://metartafapi.cbmprojects.dk";
}

// HttpClient til API kald
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Services
builder.Services.AddScoped<WeatherApiService>();
builder.Services.AddScoped(sp => new WeatherHubService(apiBaseUrl));

// LocalStorage til at gemme airports og acknowledgements
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
