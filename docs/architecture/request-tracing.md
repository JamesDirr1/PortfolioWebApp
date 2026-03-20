[Docs](../index.md) / [Architecture](index.md) / [Request Tracing](request-tracing.md)

# Request Tracing

---

# Overview

The API supports request tracing using a custom header:

```http request
Request-Id: <GUID>
```

This header allows clients and the server to correlate requests and responses for debugging and logging purposes.

## Behavior

- Clients may provide a GUID for tracing
- If not provided, the server generates one
- The value is returned in the response headers
- Used for logging and debugging

## Example

### Request

```http request
GET /api/categories HTTP/1.1
Host: example.com
Request-Id: 123e4567-e89b-12d3-a456-426614174000
``` 

### Response

```http response
HTTP/1.1 200 OK
Request-Id: 123e4567-e89b-12d3-a456-426614174000
Content-Type: application/json
```

## Implementation

The request tracing is implemented using middleware in the API layer. The middleware checks for the presence of the
`Request-Id` header, generates one if it's missing, and ensures it is included in the response headers. This allows for
consistent tracing across all requests and responses, facilitating easier debugging and monitoring of the application.

## Why Request Tracing?

Request tracing improves:

- Debugging across services
- Log correlation
- Observability of API behavior
- Performance monitoring

---

- [Back to top](#request-tracing)
- [Back to Architecture Documentation](index.md)
- [Back to Docs Home](../index.md)