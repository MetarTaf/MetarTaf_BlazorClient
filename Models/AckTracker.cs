namespace BlazorClient.Models;

/// <summary>
/// Holder styr på hvilke METAR/TAF brugeren har acknowledged.
/// Bruges til at highlighte nye rapporter.
/// </summary>
public class AckTracker
{
    private Dictionary<string, DateTime> _metarAcks = new(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, DateTime> _tafAcks = new(StringComparer.OrdinalIgnoreCase);

    public void Load(Dictionary<string, DateTime>? metarAcks, Dictionary<string, DateTime>? tafAcks)
    {
        _metarAcks = metarAcks ?? new(StringComparer.OrdinalIgnoreCase);
        _tafAcks = tafAcks ?? new(StringComparer.OrdinalIgnoreCase);
    }

    public bool IsMetarNew(string icao, DateTime reportTime)
    {
        if (!_metarAcks.TryGetValue(icao, out var acked))
            return true;
        return reportTime > acked;
    }

    public bool IsTafNew(string icao, DateTime reportTime)
    {
        if (!_tafAcks.TryGetValue(icao, out var acked))
            return true;
        return reportTime > acked;
    }

    public void AckMetar(string icao, DateTime reportTime)
    {
        _metarAcks[icao] = reportTime;
    }

    public void AckTaf(string icao, DateTime reportTime)
    {
        _tafAcks[icao] = reportTime;
    }

    public void AckAll(IEnumerable<AirportDto> airports)
    {
        foreach (var ap in airports)
        {
            if (ap.LatestMetar != null)
                AckMetar(ap.Icao, ap.LatestMetar.ReportTimeUtc);
            if (ap.LatestTaf != null)
                AckTaf(ap.Icao, ap.LatestTaf.ReportTimeUtc);
        }
    }

    public Dictionary<string, DateTime> SnapshotMetar() => new(_metarAcks);
    public Dictionary<string, DateTime> SnapshotTaf() => new(_tafAcks);
}
