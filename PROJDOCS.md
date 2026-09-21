## PeopleVille Project Documentation

PeopleVille is a simulation project with an ASP.NET Core backend, a shared C# domain-model project, and a React/Vite frontend. The repository is currently an early-stage scaffold: the frontend renders a placeholder map view, the backend exposes the default weather-forecast sample endpoint, and the core project contains initial citizen, job, banking, and building models.

## Repository Layout

```text
PeopleVille.slnx
├── PeopleVille.Core/       Shared domain models and interfaces
├── PeopleVille.Server/     ASP.NET Core web server and API controllers
└── peopleville.client/     React 19 frontend built with Vite
```

### PeopleVille.Core

The core project targets .NET 10 and has no external package dependencies. Its current types are:

- `Citizen`: name, birth date, gender, optional job, and optional current building.
- `Job`: title, salary, and work hours.
- `BankAccount`: citizen owner and balance.
- `Building`: abstract address, food inventory, and water inventory.
- `House` and `Apartment`: private homes with citizen capacity; apartments also have rent and floors.
- `ShoppingCenter`: a non-private building.
- `IPrivateHome`: contract for buildings that have citizen capacity.

### PeopleVille.Server

The server targets .NET 10 using the ASP.NET Core Web SDK. It provides controller-based APIs, development OpenAPI output, HTTPS redirection, and static-file hosting for the built client. The server falls back to `index.html` for client-side routes.

The current controller is the template `WeatherForecastController`:

```text
GET /WeatherForecast
```

It returns five randomly generated forecast records. This endpoint is currently used by the frontend as a startup/backend health check and should eventually be replaced or joined by PeopleVille simulation endpoints.

### peopleville.client
The client is an ES module React application using Vite. The current `App` component calls `/weatherforecast` and then displays a placeholder PeopleVille map with menu labels for households, slaves, and a bazaar. The Vite development server proxies `/weatherforecast` to the ASP.NET server.  ## Prerequisites - .NET 10 SDK - Node.js and npm
- A trusted local HTTPS development certificate. Vite attempts to create/export one with `dotnet dev-certs` if it is missing.

Verify the installations from the repository root:

```bash
dotnet --version
node --version
npm --version
```

## Running Locally

### Backend with SPA proxy

From the repository root:

```bash
dotnet run --project .\PeopleVille.Server\PeopleVille.Server.csproj --launch-profile https
```

The server listens on:

- HTTPS: `https://localhost:7039`
- HTTP: `http://localhost:5045`

The ASP.NET SPA proxy starts the Vite client with `npm run dev`. The configured Vite development URL is `https://localhost:50733`.

### Frontend only

Use this when working primarily on the React UI:

```bash
cd .\peopleville.client
npm install
npm run dev
```

The Vite proxy expects the backend at `https://localhost:7039` by default. Set `ASPNETCORE_HTTPS_PORT`, `ASPNETCORE_URLS`, or `DEV_SERVER_PORT` when a different local configuration is required.

## Build and Validation

Run backend compilation from the repository root:

```bash
dotnet build .\PeopleVille.slnx
```

Run frontend checks from `peopleville.client`:

```bash
npm run lint
npm run build
```

Preview the production client bundle with:

```bash
npm run preview
```

There are currently no automated test projects in the repository.

## Configuration and Conventions

- Backend configuration lives in `PeopleVille.Server/appsettings.json` and `appsettings.Development.json`.
- Launch profiles and local URLs live in `PeopleVille.Server/Properties/launchSettings.json`.
- Frontend configuration, including the API proxy and HTTPS certificate setup, lives in `peopleville.client/vite.config.js`.
- React source files are under `peopleville.client/src`; static assets belong in `peopleville.client/public` or `peopleville.client/src/assets`.
- New API controllers should be added under `PeopleVille.Server/Controllers` and use attribute routing consistent with the existing controller.
- Shared simulation concepts belong in `PeopleVille.Core`; web-specific request/response types and orchestration belong in `PeopleVille.Server`.

## Current Development Priorities

1. Define the simulation state and lifecycle for citizens, homes, jobs, money, inventory, and time.
2. Add server-side services and API endpoints for creating, reading, and advancing a simulation.
3. Replace the weather forecast startup check with a PeopleVille API health or simulation endpoint.
4. Connect the React map and menus to real API data.
5. Add persistence and automated tests once the domain behavior is defined.

## Change Log

Project-specific changes are recorded in `PeopleVille.Server/CHANGELOG.md` and `peopleville.client/CHANGELOG.md`.