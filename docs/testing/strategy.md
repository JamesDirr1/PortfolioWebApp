[Docs](../index.md) / [Testing](./index.md) / [Strategy](strategy.md)

# Strategy

---

The project focuses on testing core application logic to ensure correctness and maintainability.

- **xUnit** for unit testing
- **Moq** for mocking dependencies
- **FluentAssertions** for expressive assertions
- **Coverlet** for code coverage

This approach keeps tests focused, fast, and maintainable.

---

## Content

- [Current Scope](#current-scope)
- [Philosophy](#philosophy)
- [What is Tested](#what-is-tested)
- [Continuous Integration](#continuous-integration)

---

## Current Scope

The project includes unit tests across multiple layers:

- API layer (controllers, response handling)
- Application layer (services, mapping, orchestration)
- Domain logic (filters, rules)
- Validation (attributes and query parameters)
- Middleware (request tracing and logging)
- Infrastructure layer (repository behavior)

### Not Covered (Yet)

- Full End-to-end tests

---

## Philosophy

- All tests should pass before code is merged or pushed to a working branch
- New functionality should include corresponding unit tests
- Code coverage should be high (target ~90%), but should not prioritize coverage over meaningful tests
- Testing should focus on business logic and application behavior rather than framework internals

---

## What is Tested

The following areas are covered by tests:

### API Layer

- Controller responses and status codes
- Standardized response formatting (`ApiResponse<T>`)
- Validation responses (`ApiValidationResponse`)

### Application Layer

- Service logic and orchestration
- Mapping between domain entities and DTOs
- Pagination mapping (`PagedResult → PagedResponse`)

### Domain Layer

- Domain filters and query logic
- Core business rules

### Validation

- Custom validation attributes (e.g., `AllowedValuesAttribute`)
- Query parameter validation

### Middleware

- Request logging behavior
- Request tracing and correlation (`Request-Id`)

### Infrastructure

- Repository query behavior (filtering, sorting, pagination)

---

## What is Not Tested

The following are intentionally not tested:

- ASP.NET Core framework behavior (routing, model binding)
- EF Core internals
- Simple data models without logic

---

## Continuous Integration

Tests are executed automatically using GitHub Actions.  
See [CI](ci.md) for more information.

This ensures:

- New changes do not break existing functionality
- Code remains stable over time
- Test coverage is maintained across the project

---

- [Back to top](#strategy)
- [Back to Testing](./index.md)
- [Back to Docs Home](../index.md)
