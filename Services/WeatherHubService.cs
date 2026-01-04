using Microsoft.AspNetCore.SignalR.Client;
using BlazorClient.Models;

namespace BlazorClient.Services;

public class WeatherHubService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly string _hubUrl;
    private readonly HashSet<string> _subscribedIcaos = new(StringComparer.OrdinalIgnoreCase);

    public event Action<WeatherUpdateDto>? OnWeatherUpdate;
    public event Action? OnConnected;
    public event Action? OnDisconnected;

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    public WeatherHubService(string baseUrl)
    {
        _hubUrl = $"{baseUrl.TrimEnd('/')}/hubs/weather";
    }

    public async Task ConnectAsync()
    {
        if (_connection != null)
            return;

        _connection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
            .Build();

        _connection.On<WeatherUpdateDto>("WeatherUpdate", update =>
        {
            Console.WriteLine($"[SignalR] WeatherUpdate for {update.Icao}");
            OnWeatherUpdate?.Invoke(update);
        });

        _connection.Reconnected += async _ =>
        {
            Console.WriteLine("[SignalR] Reconnected - resubscribing...");
            foreach (var icao in _subscribedIcaos)
            {
                await _connection.InvokeAsync("SubscribeToAirport", icao);
            }
            OnConnected?.Invoke();
        };

        _connection.Closed += _ =>
        {
            Console.WriteLine("[SignalR] Connection closed");
            OnDisconnected?.Invoke();
            return Task.CompletedTask;
        };

        try
        {
            await _connection.StartAsync();
            Console.WriteLine("[SignalR] Connected");
            OnConnected?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SignalR] Connection failed: {ex.Message}");
            throw;
        }
    }

    public async Task SubscribeToAirportAsync(string icao)
    {
        if (_connection?.State != HubConnectionState.Connected)
            return;

        icao = icao.ToUpperInvariant();
        await _connection.InvokeAsync("SubscribeToAirport", icao);
        _subscribedIcaos.Add(icao);
        Console.WriteLine($"[SignalR] Subscribed to {icao}");
    }

    public async Task UnsubscribeFromAirportAsync(string icao)
    {
        if (_connection?.State != HubConnectionState.Connected)
            return;

        icao = icao.ToUpperInvariant();
        await _connection.InvokeAsync("UnsubscribeFromAirport", icao);
        _subscribedIcaos.Remove(icao);
        Console.WriteLine($"[SignalR] Unsubscribed from {icao}");
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
