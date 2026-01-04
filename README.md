# CAVOK - Blazor WebAssembly Frontend

Blazor WebAssembly frontend til METAR/TAF overvågning med SignalR push-notifikationer.

## Kom i gang

### Udvikling (lokal)

```bash
cd BlazorClient
dotnet run
```

Frontend peger automatisk på pre-API (`192.168.1.162:5001`) via `appsettings.Development.json`.

### Produktion/Pre (server)

Frontend og API hostes sammen - ingen konfiguration nødvendig. 
Relative URLs bruges automatisk når `ApiBaseUrl` er tom.

## Konfiguration

```
wwwroot/
├── appsettings.json              # Produktion: tom URL (relativ)
└── appsettings.Development.json  # Udvikling: peger på pre-API
```

**appsettings.json** (Pre/Prod - hostet sammen med API):
```json
{
  "ApiBaseUrl": ""
}
```

**appsettings.Development.json** (Lokal udvikling):
```json
{
  "ApiBaseUrl": "http://192.168.1.162:5001"
}
```

## Miljøer

| Miljø | Frontend | API | Config |
|-------|----------|-----|--------|
| Udvikling | localhost | 192.168.1.162:5001 | Development.json |
| Pre | Samme host | Samme host | appsettings.json (tom) |
| Prod | Samme host | Samme host | appsettings.json (tom) |

## Features

- ✅ Tilføj/fjern lufthavne
- ✅ Real-time updates via SignalR
- ✅ Acknowledge nye rapporter
- ✅ AMD-alerts med blinkende rød border
- ✅ METAR/TAF vises i fuld længde (vertikal stak)
- ✅ Gemmer lufthavne i localStorage
- ✅ Gemmer acknowledgements i localStorage

## Arkitektur

```
BlazorClient/
├── Layout/
│   └── MainLayout.razor
├── Models/
│   ├── AirportModels.cs    # DTOs
│   └── AckTracker.cs       # Tracker for acknowledge
├── Pages/
│   └── OverviewPage.razor  # Hovedsiden
├── Services/
│   ├── WeatherApiService.cs    # HTTP kald til API
│   └── WeatherHubService.cs    # SignalR forbindelse
├── wwwroot/
│   ├── css/app.css
│   ├── index.html
│   └── appsettings.json
├── App.razor
├── Program.cs
└── _Imports.razor
```

## Layout

Hver lufthavn vises som et kort med:
- Header med ICAO + land + knapper
- TAF sektion (vises først da den typisk er længere)
- METAR sektion

Rapporterne vises i fuld længde uden line-break (dog med word-wrap ved smalle skærme).

## Styling

- Nye rapporter highlightes med gul baggrund
- AMD highlightes med rød baggrund + blinkende border
- SPECI, COR, AMD badges vises ved rapporttypen
