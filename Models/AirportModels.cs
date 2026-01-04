namespace BlazorClient.Models;

public class AirportDto
{
    public string Icao { get; set; } = string.Empty;
    public string? Country { get; set; }
    public MetarDto? LatestMetar { get; set; }
    public TafDto? LatestTaf { get; set; }
}

public class MetarDto
{
    public DateTime ReportTimeUtc { get; set; }
    public DateTime FetchTimeUtc { get; set; }
    public string Raw { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class TafDto
{
    public DateTime ReportTimeUtc { get; set; }
    public DateTime FetchTimeUtc { get; set; }
    public string Raw { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsAmendment { get; set; }
}

public class WeatherUpdateDto
{
    public string Icao { get; set; } = string.Empty;
    public MetarDto? NewMetar { get; set; }
    public TafDto? NewTaf { get; set; }
    public DateTime UpdateTimeUtc { get; set; }
}
