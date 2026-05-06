[Docs](../index.md) / [API](./index.md)

# API Documentation

---

This section documents the public API for the Portfolio Web App. It includes available endpoints, shared response
formats, request tracing behavior, validation error structure, pagination standards, and example usage.

The API is built with **ASP.NET Core Web API** and uses **JSON** for request and response bodies.

---

## Base URL

> http://localhost:5018

When running locally, Swagger UI is available in development mode.

> http://localhost:5018/swagger

---

## API Standards

### Content Type

All API responses are returned as **JSON**.

> Content-Type: application/json

Endpoints that accept request bodies also expect **JSON**.

> Content-Type: application/json

### JSON Naming Convention

The API uses **camelCase** for all JSON property names in requests and responses.

Example:

```json
{
  "displayOrder": 1,
  "isActive": true
}
```

### Standard Response Envelopes

All endpoints return a standardized response structure.

- Successful and general API responses use `ApiResponse<T>`
- Validation failures use `ApiValidationResponse`

### Pagination

Paginated endpoints return pagination metadata inside the `data.metaData` object.

```json
{
  "data": {
    "items": [],
    "metaData": {}
  }
}
```

---

## Request Tracing

The API supports request tracing through the `Request-Id` header.

| Header     | Required | Description                                                                            |
|------------|----------|----------------------------------------------------------------------------------------|
| Request-Id | No       | Optional GUID used for request tracing. If not provided, the server will generate one. |

Clients may provide a `Request-Id` header to make debugging easier across logs, requests, and responses.

Example:

```http request
GET /api/categories HTTP/1.1
Host: localhost:5018
Request-Id: 47ff3631-c08c-4a47-9dc3-32dd628d4a49
```

If a `Request-Id` is not provided, the server generates one and returns it in the response headers.

---

## Standard API Response Format

Most successful API responses use a standard response envelope.

```json
{
  "success": true,
  "message": "Request successful.",
  "data": {},
  "errors": []
}
```

### Response Fields

| Field     | Type    | Description                                     |
|-----------|---------|-------------------------------------------------|
| `success` | boolean | Indicates whether the request succeeded.        |
| `message` | string  | Human-readable result message.                  |
| `data`    | object  | Response payload. May be `null` for failures.   |
| `errors`  | array   | Error messages for non-validation API failures. |

### Example Successful Response

```json
{
  "success": true,
  "message": "Categories retrieved successfully.",
  "data": {
    "items": []
  },
  "errors": []
}
```

### Example Failure Response

```json
{
  "success": false,
  "message": "An unexpected error occurred.",
  "data": null,
  "errors": [
    "Please try again later."
  ]
}
```

---

## Validation Error Format

Validation errors use a validation-specific response envelope. This keeps field-level validation errors easy to read and
easy for frontend clients to display.

```json
{
  "success": false,
  "message": "Validation failed.",
  "data": null,
  "errors": {
    "FieldName": [
      "Validation error message."
    ]
  }
}
```

### Validation Response Fields

| Field     | Type    | Description                                              |
|-----------|---------|----------------------------------------------------------|
| `success` | boolean | Always `false` for validation failures.                  |
| `message` | string  | Validation failure message.                              |
| `data`    | null    | Always `null` for validation failures.                   |
| `errors`  | object  | Dictionary of field names mapped to validation messages. |

### Example Validation Error

```http
GET /api/categories?sortBy=CreatedDate HTTP/1.1
Host: localhost:5018
```

```json
{
  "success": false,
  "message": "Validation failed.",
  "data": null,
  "errors": {
    "SortBy": [
      "SortBy must be one of: Id, Title, DisplayOrder."
    ]
  }
}
```

---

## Pagination Format

List endpoints that support pagination return paged data inside the standard API response envelope.

```json
{
  "success": true,
  "message": "Categories retrieved successfully.",
  "data": {
    "items": [],
    "metaData": {
      "page": 1,
      "pageSize": 10,
      "totalCount": 0,
      "totalPages": 0,
      "hasPreviousPage": false,
      "hasNextPage": false
    }
  },
  "errors": []
}
```

### Paged Response Fields

| Field      | Type   | Description             |
|------------|--------|-------------------------|
| `items`    | array  | Records for the page.   |
| `metaData` | object | Pagination information. |

### Pagination Metadata Fields

| Field             | Type    | Description                               |
|-------------------|---------|-------------------------------------------|
| `page`            | integer | Current page number.                      |
| `pageSize`        | integer | Number of records requested per page.     |
| `totalCount`      | integer | Total number of matching records.         |
| `totalPages`      | integer | Total number of pages available.          |
| `hasPreviousPage` | boolean | Indicates whether a previous page exists. |
| `hasNextPage`     | boolean | Indicates whether a next page exists.     |

---

## Common Query Parameters

Some list endpoints support filtering, sorting, and pagination.

| Parameter       | Type    | Required | Description                               |
|-----------------|---------|----------|-------------------------------------------|
| `page`          | integer | No       | Page number to retrieve (default: 1).     |
| `pageSize`      | integer | No       | Number of records per page (default: 10). |
| `sortBy`        | string  | No       | Field used to sort results.               |
| `sortDirection` | string  | No       | Sort direction: `asc` or `desc`.          |

Endpoint-specific filters are documented on each resource page.

---

## Status Codes

| Status Code                 | Meaning          | Description                                     |
|-----------------------------|------------------|-------------------------------------------------|
| `200 OK`                    | Success          | Request completed successfully.                 |
| `400 Bad Request`           | Validation Error | Query string, route, or body validation failed. |
| `404 Not Found`             | Missing Resource | Requested resource could not be found.          |
| `500 Internal Server Error` | Server Error     | An unexpected server error occurred.            |

---

## Public API Endpoints

> These endpoints are accessible without authentication.

| Resource   | Documentation                      | Description                                                               |
|------------|------------------------------------|---------------------------------------------------------------------------|
| Categories | [Categories](public/categories.md) | Retrieve active project categories (e.g., Comics, Painting, Photography). |

---

## Future API Endpoints

The following endpoints are planned but not yet documented:

| Resource                 | Status |
|--------------------------|--------|
| Projects                 | TODO   |
| Images                   | TODO   |
| Authentication           | TODO   |
| Admin Content Management | TODO   |

## Notes

- Use the `success` field to quickly determine whether the request succeeded.
- Use the `message` field for user-friendly status messages when appropriate.
- Use `data.items` for paginated list records.
- Use `data.metaData` to build pagination controls.
- Use validation `errors` to display field-level form or query parameter errors.
- Include a `Request-Id` header when debugging client-server issues.

---

- [Back to top](#api-documentation)
- [Back to Docs Home](../index.md)

