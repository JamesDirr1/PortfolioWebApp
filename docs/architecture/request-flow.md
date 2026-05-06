[Docs](../index.md) / [Architecture](./index.md) / [Request Flow](request-flow.md)

# Request Flow

---

This document describes how a request flows through the application, from the client to the database and back.

---

## Flow

A typical request follows this path:

The following diagrams illustrate the request lifecycle across application layers.

### High-Level Flow Diagram

![Request Flow](../_assets/request-flow.png)
<details>
<summary>View Mermaid Source</summary>

```mermaid
flowchart TB
    Client["Client"]

    subgraph API["API Layer"]
        MiddlewareStart["Middleware Start"]
        Controller["Categories Controller"]
        ApiResponse["API Response"]
        MiddlewareEnd["Middleware End"]
    end

    subgraph Application["Application Layer"]
        QueryParams["Query Parameters"]
        Service["Category Service"]
        Mapping["Paged Response Mapping"]
    end

    subgraph Domain["Domain Layer"]
        Filter["Category Filter"]
        PagedResult["Paged Result"]
    end

    subgraph Infrastructure["Infrastructure Layer"]
        Repository["Category Repository"]
        DbContext["App DbContext"]
        PostgreSQL["PostgreSQL"]
    end

    Client --> MiddlewareStart
    MiddlewareStart --> Controller
    Controller --> QueryParams
    QueryParams --> Service
    Service --> Filter
    Filter --> Repository
    Repository --> DbContext
    DbContext --> PostgreSQL

    PostgreSQL --> Repository
    Repository --> PagedResult
    PagedResult --> Service
    Service --> Mapping
    Mapping --> ApiResponse
    ApiResponse --> MiddlewareEnd
    MiddlewareEnd --> Client
```

</details>

### Sequence Diagram

![Sequence Diagram](../_assets/sequence-diagram.png)
<details>
<summary>View Mermaid Source</summary>

```mermaid
sequenceDiagram
    participant Client
    participant Middleware as Middleware
    participant Controller as CategoriesController
    participant Service as CategoryService
    participant Domain as CategoryFilter
    participant Repository as CategoryRepository
    participant DbContext as AppDbContext
    participant DB as PostgreSQL

    Client->>Middleware: GET /api/categories?page=1&pageSize=10
    Middleware->>Middleware: Read/generate Request-Id
    Middleware->>Middleware: Log request start

    Middleware->>Controller: Forward request
    Controller->>Controller: Bind CategoryQueryParameters
    Controller->>Controller: Validate query parameters

    Controller->>Service: GetAllAsync(query, cancellationToken)
    Service->>Domain: Map query parameters to CategoryFilter
    Service->>Repository: GetAllAsync(filter, cancellationToken)

    Repository->>DbContext: Build EF Core query
    DbContext->>DB: Execute SQL query
    DB-->>DbContext: Return category rows
    DbContext-->>Repository: Return entities

    Repository-->>Service: Return PagedResult<Category>
    Service->>Service: Map Category entities to CategoryDto
    Service->>Service: Create PagedResponse<CategoryDto>

    Service-->>Controller: Return PagedResponse<CategoryDto>
    Controller->>Controller: Wrap in ApiResponse<T>

    Controller-->>Middleware: Return HTTP response
    Middleware->>Middleware: Log status code and duration
    Middleware->>Middleware: Add Request-Id response header
    Middleware-->>Client: 200 OK + JSON response
```

</details>

---

## Step-by-Step Example (Categories)

### 1. Client Request

The client sends a request:

> GET /api/categories

### 2. Middleware  (Request Start)

Middleware processes the request before it reaches the controller.

This includes:

- Generating or reading the `Request-Id`
- Logging request details using Serilog

### 3. Controller (API Layer)

- Receives the HTTP request
- Binds query parameters into `CategoryQueryParameters`
- Model validation runs automatically
    - Invalid requests return a validation response
- Calls the Application service

### 4. Application Service

- Normalizes query parameters (trimming, defaults)
- Maps query parameters → `CategoryFilter`
- Calls the repository layer

### 5. Domain Filter

- Encapsulates filtering, sorting, and pagination rules
- Separates domain logic from API concerns

### 6. Repository (Infrastructure Layer)

- Uses EF Core to build database queries
- Applies filtering, sorting, and pagination
- Executes queries against PostgreSQL
- Returns a `PagedResult<T>`

### 7. Application Mapping

- Maps domain entities → DTOs
- Converts `PagedResult<T>` → `PagedResponse<T>`
- Adds pagination metadata

### 8. API Response Wrapping

- The result is wrapped in `ApiResponse<T>`
- Adds `success`, `message`, `data`, and `errors`

### 9. Middleware (Response End)

After the downstream pipeline completes, control returns to the middleware.

This allows the middleware to:

- Read the final response status code
- Stop the timer and calculate elapsed time
- Log completion details for the request
- Ensure the `Request-Id` is included in the response headers

### 10. Response

- The completed HTTP response is returned to the client

---

## Summary:

- **Middleware** handles cross-cutting concerns (logging, tracing)
- **Controller** handles HTTP + validation
- **Application** handles business logic + mapping
- **Domain** defines rules and filters
- **Infrastructure** executes queries
- **Responses** are standardized and consistent

---

- [Back to top](#request-flow)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)