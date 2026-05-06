[Docs](../index.md) / [Architecture](./index.md) / [Data Access](data-access.md)

# Data Access

---

The application uses **Entity Framework Core** with **PostgreSQL** for data persistence.

Entity Framework Core acts as an Object-Relational Mapper (ORM), allowing the application to interact with the database
using C# objects instead of raw SQL.

---

## Configuration

The database connection is configured using connection strings defined in:

```text
appsettings.json
```

These settings are loaded during application startup and used to configure the EF Core `DbContext`.

---

## DbContext

The `DbContext` represents a session with the database and is responsible for:

- Querying data
- Tracking changes
- Saving updates

It is defined in the Infrastructure layer and registered using dependency injection.

---

## Repository Pattern

The application uses a repository pattern to abstract data access logic.

**Responsibilities:**

- Encapsulate EF Core queries
- Apply filtering, sorting, and pagination
- Return domain-level results (`PagedResult<T>`)

Example responsibilities in CategoryRepository:

- Filtering using `Title`
- Sorting by `Id`, `Title`, or `DisplayOrder`
- Pagination using `Page` and `PageSize`

**Benefits:**

- Keeps data access logic isolated from business logic
- Improves testability
- Allows the Application layer to remain database-agnostic

---

## Query Behavior

### Filtering

Filtering is performed using EF Core expressions:

- Case-insensitive matching is implemented using `EF.Functions.ILike`
- Empty or whitespace values are ignored

### Sorting

Sorting is dynamically applied based on query parameters:

- Supported fields are defined in `QueryParameters`
- Direction: ascending or descending

### Pagination

Pagination is implemented using:

```csharp
query.Skip((page - 1) * pageSize).Take(pageSize);
```

The repository also calculates the total count to support pagination metadata.

---

## Performance Considerations

To optimize read operations:

- `AsNoTracking()` is used for queries that do not modify entities
- Only required data is selected and mapped
- Pagination limits the number of records returned

These practices help reduce memory usage and improve query performance.

---

## Migrations

Database schema changes are managed using EF Core migrations.

- Migrations are created and applied from the Infrastructure layer
- They allow version-controlled updates to the database schema
- Ensures consistency across development environments

---

## Local Development

Docker is used to run a local PostgreSQL instance.
This allows:

- Consistent development environments
- Easy setup without manual database installation
- Isolation from the host system

--- 

- [Back to top](#data-access)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)