using System.Net.Http.Json;
using BlazorClient.Models;

namespace BlazorClient.Services;

public class WeatherApiService
{
    private readonly HttpClient _http;

    public WeatherApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AirportDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<AirportDto>>("api/airports");
        return result ?? new List<AirportDto>();
    }

    public async Task<AirportDto?> GetAsync(string icao)
    {
        try
        {
            return await _http.GetFromJsonAsync<AirportDto>($"api/airports/{icao}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<AirportDto?> SubscribeAsync(string icao)
    {
        var response = await _http.PostAsync($"api/airports/subscriptions/{icao}", null);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AirportDto>();
    }

    public async Task<bool> UnsubscribeAsync(string icao)
    {
        var response = await _http.DeleteAsync($"api/airports/subscriptions/{icao}");
        return response.IsSuccessStatusCode;
    }

    public async Task ForceFetchAsync()
    {
        await _http.PostAsync("api/airports/fetch", null);
    }
}
