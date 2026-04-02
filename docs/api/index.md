[Docs](../index.md) / [API](docs/api/index.md)

# API Documentation

---

This section provides detailed documentation for the API endpoints of the project. It includes information about the
available routes, request and response formats, authentication requirements, and example usage.

---

## Notes

- All endpoints return JSON responses and expect JSON request bodies where applicable.
- Request tracking is implemented for better debugging and monitoring via `Request-Id` header.
- Authentication is required for protected endpoints, while public endpoints are accessible without authentication.
- Request and response examples are provided for each endpoint to facilitate integration.
- Endpoints are organized by resource type (e.g., Categories, Projects, Images) for better navigation.

---

## Base URL

http://localhost:5018

---

## Public API Endpoints

> These endpoints are accessible without authentication.

- [Categories](public/categories.md) - Retrieve active project categories (e.g., Comics, Painting, Photography).

---

## Future API Endpoints

- [Projects](projects.md)
- [Images](images.md)

---

- [Back to top](#api-documentation)
- [Back to Docs Home](../index.md)

