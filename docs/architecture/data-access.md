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