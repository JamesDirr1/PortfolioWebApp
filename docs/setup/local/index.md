[Docs](../../index.md) / [setup](../index.md) / [Run Locally](./index.md)

# Run Locally

---

This guide explains how to set up and run the application locally for development.

___

## Contents

- [Quick Start](#quick-start) - Quick commands to get everything up and running
- [First Run](#first-run) - Walkthrough for first time setup
- [Normal Run](#normal-run) - Running the application after initial setup
- [Notes](#notes) - Additional information

---

## Quick Start

Starts the database and API with minimal setup.

```bash
docker compose up -d
dotnet run --project src/backend/PortfolioWebApp.Api
```

---

## First Run

This section walks through setting up the project for the first time.

These steps are based on Windows, but should be similar for other operating systems.

### Prerequisites

Ensure that you have all of the following installed.

- [Git](https://git-scm.com/install/)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://docs.docker.com/desktop/)

If you are new to Git, see:

- [First-Time Git Setup](https://git-scm.com/book/en/v2/Getting-Started-First-Time-Git-Setup)
- [GitHub Guide](https://github.com/git-guides)

### Clone Repository

Navigate to the directory where you want the project to live.

For example:

```bash
cd C:\Users\james\dev\PortfolioWebApp
``` 

Clone the repository:

```bash
git clone https://github.com/JamesDirr1/PortfolioWebApp
```

### (Optional) Switch Branch

The default branch is `main`, which contains the latest stable version.

To switch branches:

```bash
git switch version/0.1.0
```

See [Changelog](../../changelog/index.md) for available version.

### Database setup

Since the database runs via a **Docker** container, setup is straightforward.

Start the database:

```bash
docker compose up -d
```

Ensure that Docker Desktop is running before executing this command.

Apply migrations:

```bash
dotnet ef database update
```

This command applies database migrations and sets up the schema.

It typically only needs to be run during initial setup or when the database has been reset.

### Start API

Run the API using:

```bash
dotnet run --project src/backend/PortfolioWebApp.Api
```

The API should now be running and ready to accept requests.
You can test endpoints using the [API Documentation](../../api/index.md)

___

## Normal Run

After completing the initial setup, running the application locally requires only two commands.

### Start the Database

Ensure Docker Desktop is running, then start the database:

```bash
docker compose up -d
```

### Run the API

Start the API:

```bash
dotnet run --project src/backend/PortfolioWebApp.Api
```

The API should now be running and ready to accept requests.
You can test endpoints using the [API Documentation](../../api/index.md)

---

## Notes

- You can adjust log levels for **Serilog** via `src/backend/PortfolioWebApp.Api/appsettings.json`
- For instructions on running tests, see [Testing](../../testing/index.md)
- The database container will persist between runs unless removed
- Migrations do not need to be re-applied unless the database is reset

---

- [Back to top](#run-locally)
- [Back to Setup](../index.md)
- [Back to Docs Home](../../index.md)