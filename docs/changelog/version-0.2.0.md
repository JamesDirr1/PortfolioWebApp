[Docs](../index.md) / [Changelog](index.md) / [Version 0.2.0](version-0.2.0.md)

# Version 0.2.0

---

Expanded the Categories API with filtering, sorting, pagination, validation, standardized API responses, improved
logging, and comprehensive testing across all layers.

---

## Added

### Core Application

- Enhanced Categories endpoint with:
    - Filtering by title
    - Sorting (`Id`, `Title`, `DisplayOrder`)
    - Pagination (`page`, `pageSize`)
- Query parameter model (`CategoryQueryParameters`)
- Domain filtering model (`CategoryFilter`)
- Pagination models:
    - `PagedResult<T>` (Domain)
    - `PagedResponse<T>` and `PagedMetaData` (Application)

### API Design

- Standardized response envelope (`ApiResponse<T>`)
- Validation response model (`ApiValidationResponse`)
- Consistent API response formatting across endpoints
- Standardized `404 Not Found` API responses
- Invalid route parameter handling for category IDs
- Query parameter validation using `AllowedValuesAttribute`
- Controller response helper methods:
    - `Success<T>()`
    - `Failure<T>()`
    - `FailureNotFound<T>()`

### Testing

- Expanded test coverage across layers:
    - API controllers
    - Application services
    - Domain logic
    - Validation attributes
    - Middleware (logging and request tracing)
    - Infrastructure (repository behavior)
- Infrastructure tests using Testcontainers with PostgreSQL

### Infrastructure

- Improved repository implementation:
    - Filtering
    - Sorting
    - Pagination
- Use of `AsNoTracking()` for read performance
- Case-insensitive filtering using `EF.Functions.ILike`

### Documentation

- Updated API documentation with:
    - Request/response examples
    - Standard response envelope documentation
    - Validation response documentation
    - Pagination metadata documentation
    - API conventions and standards
    - Invalid ID and error response examples
- Added architecture diagrams (flow + sequence)
- Expanded documentation for:
    - Request flow
    - Logging
    - Request tracing
    - Data access
    - Testing strategy

---

- [Back to top](#version020)
- [Back to change log](./index.md)
- [Back to Docs Home](../index.md)