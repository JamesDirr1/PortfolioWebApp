[Docs](../index.md) / [Architecture](./index.md) / [Design Decisions](decisions.md)

# Design Decisions

---

## Contents

- [Why make this project?](#why-make-this-project)
    - [Why not just modify the original project?](#why-not-just-modify-the-original-project)
- [Why ASP.NET Core?](#why-aspnet-core)
- [Why PostgreSQL?](#why-postgresql)
- [Why layered architecture?](#why-layered-architecture)
- [Why standardized API responses?](#why-standardized-api-responses)

--- 

## Why make this project?

To learn and practice modern backend development in C# using ASP.NET Core, Entity Framework Core, and PostgreSQL, while
applying layered architecture principles.

I wanted to create a real-world project that could serve as a portfolio piece and demonstrate my skills in backend
development, API design, and software architecture.

### Why not just modify the original project?

The original project was built with Flask and MySQL without a focus on scalability or maintainability.
Modifying it would have been constrained by earlier design decisions and would not have allowed me to apply best
practices from the ground up.
Rebuilding it gave me the opportunity to create a more structured, maintainable, and scalable application while learning
new technologies.

---

## Why ASP.NET Core

Chosen to learn modern backend development in C# to align with my career goals and explore a new tech stack.

**ASP.NET Core provides:**

- Strong performance and scalability
- Built-in dependency injection
- A robust middleware pipeline
- Excellent ecosystem support for building APIs

---

## Why PostgreSQL

A reliable relational database with strong EF Core support.

It also provides experience with a different database system beyond my previous work with MySQL and MS SQL.

**PostgreSQL offers:**

- Advanced querying capabilities
- Strong support for indexing and performance tuning
- Compatibility with modern cloud environments

---

## Why layered architecture

The application uses a layered architecture to enforce separation of concerns and maintain clear boundaries between
responsibilities.

**Each layer has a distinct role:**

- **API**: Handles HTTP concerns, validation, and response formatting
- **Application**: Contains business logic and orchestration
- **Domain**: Defines core models and business rules
- **Infrastructure**: Handles data access and external systems

**Benefits:**

- Improves testability by isolating logic
- Reduces coupling between components
- Enables easier refactoring and scalability
- Allows independent evolution of each layer

This structure ensures that changes in one layer (e.g., database or API format) do not directly impact others.

---

## Why standardized API responses

The API uses a consistent response envelope (`ApiResponse<T>`) for all successful and failed requests.

**Benefits:**

- Ensures consistent response structure across endpoints
- Simplifies frontend integration
- Makes error handling predictable
- Improves maintainability as the API grows

Validation errors use a separate structure (`ApiValidationResponse`) to provide field-level error details.

---

- [Back to top](#design-decisions)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)
