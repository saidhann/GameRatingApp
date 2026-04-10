# 🎮 GameRatingApp

A full-stack web application for browsing and rating video games, built with **Blazor WebAssembly**, **ASP.NET Core Web API**, and **MySQL**. The solution follows a clean three-project architecture: a shared class library, a REST API backend, and a Blazor frontend client.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Setup](#database-setup)
  - [Running the API](#running-the-api)
  - [Running the Blazor Client](#running-the-blazor-client)
- [Docker](#docker)
- [API Overview](#api-overview)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

GameRatingApp allows users to browse a catalog of games and submit ratings. The frontend is a **Blazor WebAssembly** SPA communicating with a **RESTful ASP.NET Core API**, which reads and writes to a **MySQL** database. Shared data models and DTOs live in a separate **ClassLibrary** project referenced by both the API and the client.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor WebAssembly (.NET) |
| Backend | ASP.NET Core Web API (.NET) |
| Database | MySQL |
| Shared Models | C# Class Library |
| Containerization | Docker |
| IDE | Visual Studio 2022 |

---

## Architecture

```
┌─────────────────────┐        HTTP/REST        ┌──────────────────────┐
│   MyBlazorClient    │ ──────────────────────► │    DatabaseApi       │
│  (Blazor WASM SPA)  │ ◄────────────────────── │  (ASP.NET Core API)  │
└─────────────────────┘       JSON responses     └──────────┬───────────┘
           │                                                │
           └──────────────┐                    ┌───────────┘
                          ▼                    ▼
                   ┌─────────────────────────────┐
                   │       ClassLibrary           │
                   │  (Shared Models & DTOs)      │
                   └──────────────┬──────────────┘
                                  │
                                  ▼
                          ┌───────────────┐
                          │     MySQL     │
                          │   Database    │
                          └───────────────┘
```

---

## Project Structure

```
GameRatingApp/
├── MyBlazorClient/          # Blazor WebAssembly frontend
│   ├── Pages/               # Razor pages & components
│   ├── wwwroot/             # Static assets
│   └── MyBlazorClient.csproj
│
├── DatabaseApi/             # ASP.NET Core REST API
│   ├── Controllers/         # API controllers
│   ├── Services/            # Business logic / data access
│   ├── Dockerfile           # Container definition for the API
│   └── DatabaseApi.csproj
│
├── ClassLibrary/            # Shared models and DTOs
│   └── ClassLibrary.csproj
│
├── MyBlazorClient.sln       # Visual Studio solution file
├── .gitignore
└── .gitattributes
```

---

## Getting Started

### Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code with C# extension
- (Optional) [Docker](https://www.docker.com/) for containerized deployment

### Database Setup

1. Create a MySQL database:

```sql
CREATE DATABASE GameRatingDb;
```

2. Update the connection string in `DatabaseApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GameRatingDb;User=root;Password=yourpassword;"
  }
}
```

3. Apply any migrations or seed scripts located in the `DatabaseApi` project:

```bash
cd DatabaseApi
dotnet ef database update
```

### Running the API

```bash
cd DatabaseApi
dotnet run
```

The API will start at `https://localhost:7XXX` (check your terminal output for the exact port).

### Running the Blazor Client

Open a new terminal:

```bash
cd MyBlazorClient
dotnet run
```

Then open your browser and navigate to `https://localhost:5XXX`. The client will connect to the API automatically.

> **Tip:** You can also open `MyBlazorClient.sln` in Visual Studio 2022 and run both projects simultaneously using the Multiple Startup Projects setting.

---

## Docker

The `DatabaseApi` project includes a `Dockerfile` for containerized deployment.

### Build and run the API container

```bash
cd DatabaseApi
docker build -t gameratingapp-api .
docker run -p 8080:80 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=GameRatingDb;User=root;Password=yourpassword;" \
  gameratingapp-api
```

> Make sure your MySQL server is accessible from within the container. Use `host.docker.internal` to reach the host machine's MySQL from Docker on Windows/macOS, or `172.17.0.1` on Linux.

---

## API Overview

The `DatabaseApi` exposes a RESTful API consumed by the Blazor client. Typical endpoints include:

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/games` | Get all games |
| `GET` | `/api/games/{id}` | Get a game by ID |
| `POST` | `/api/games` | Add a new game |
| `POST` | `/api/ratings` | Submit a rating for a game |
| `GET` | `/api/ratings/{gameId}` | Get ratings for a game |

> These are inferred from the project structure. Verify against the actual controller files for exact routes.

---

## Contributing

Contributions are welcome!

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

---

## License

This project does not currently specify a license. Please contact the author before using or distributing this work.

---

> Built with ❤️ using Blazor, ASP.NET Core, and MySQL.
