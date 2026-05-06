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

Responsible for handling HTTP requests, responses, and cross-cutting concerns.

### Key responsibilities:

- Controllers (entry point for HTTP requests)
- Routing and endpoint definitions
- Standardized API responses (ApiResponse<T>, ApiValidationResponse)
- Middleware (request tracing, logging, error handling)
- Model validation handling and error formatting
- Application configuration (Program.cs, appsettings.json)

---

## Application Layer `PortfolioWebApp.Application`

Contains business logic, orchestration, and application-level abstractions.

### Key responsibilities:

- Services (e.g., CategoryService)
- DTOs (Data Transfer Objects)
- Query parameter models (e.g., CategoryQueryParameters)
- Pagination models (PagedResponse<T>, PagedMetaData)
- Validation attributes and rules (e.g., AllowedValuesAttribute)
- Interfaces for repositories and services

### Notes

Responsible for mapping:

Domain → DTOs
Repository results → API-friendly responses

---

## Domain Layer `PortfolioWebApp.Domain`

Represents the core business model and rules.

### Key responsibilities:

- Entities (e.g., Category, Project)
- Domain queries/filters (e.g., CategoryFilter)
- Core abstractions (e.g., repository interfaces)
- Domain-level pagination results (PagedResult<T>)

This layer contains no external dependencies and represents the “source of truth” for business concepts.

---

## Infrastructure Layer `PortfolioWebApp.Infrastructure`

Handles external systems such as the database and persistence logic.

### Key responsibilities:

- Entity Framework Core DbContext
- Repository implementations (e.g., CategoryRepository)
- Query execution (filtering, sorting, pagination)
- Database migrations
- PostgreSQL integration

---

- [Back to Layers](#project-structure)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)