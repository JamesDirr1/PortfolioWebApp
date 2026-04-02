[Docs](../index.md) / [Architecture](./index.md) / [Project Structure](project-structure.md)

# Project Structure

---
The application is divided into four layers:

- [API](#api-layer-portfoliowebappapi)
- [Application](#application-layer-portfoliowebappapplication)
- [Domain](#domain-layer-portfoliowebappdomain)
- [Infrastructure](#infrastructure-layer-portfoliowebappinfrastructure)

Layout of the project structure:

```text
src/backend/
├── PortfolioWebApp.Api
├── PortfolioWebApp.Application
├── PortfolioWebApp.Domain
└── PortfolioWebApp.Infrastructure
```

---

## API Layer `PortfolioWebApp.Api`

Responsible for handling HTTP requests and responses.

### Key responsibilities:

- Controllers
- Routing
- Request/response formatting
- Middleware (e.g., request tracing via X-Request-Id)
- Application configuration (Program.cs, appsettings.json)

---

## Application Layer `PortfolioWebApp.Application`

Contains business logic and application services.

### Key responsibilities:

- Services (business logic)
- DTOs (Data Transfer Objects)
- Interfaces (abstractions for infrastructure)
- Validation logic

---

## Domain Layer `PortfolioWebApp.Domain`

Represents the core business model of the application.

### Key responsibilities:

- Entities (e.g., Category, Project)
- Domain rules
- Core abstractions

This layer contains no external dependencies and represents the “source of truth” for business concepts.

---

## Infrastructure Layer `PortfolioWebApp.Infrastructure`

Handles external systems and persistence.

Key responsibilities:

- Entity Framework Core DbContext
- Repository implementations
- Database migrations
- PostgreSQL integration

---

- [Back to Layers](#project-structure)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)