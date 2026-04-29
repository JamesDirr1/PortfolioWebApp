[Docs](../index.md) / [Architecture](./index.md) / [Request Tracing](request-tracing.md)

# Request Tracing

---

The API supports request tracing using a custom header:

```http
Request-Id: <GUID>
```

This header allows clients and the server to correlate requests and responses for debugging and logging purposes.

---

## Behavior

- Clients may provide a GUID for request tracing
- If not provided, the server generates a unique identifier
- The value is returned in the response headers
- The same ID is used across logs for the request lifecycle
- Used for logging, debugging, and request correlation

---

## Example

### Request

```http request
GET /api/categories HTTP/1.1
Host: localhost:5018
Request-Id: 123e4567-e89b-12d3-a456-426614174000
``` 

### Response

```http response
HTTP/1.1 200 OK
Request-Id: 123e4567-e89b-12d3-a456-426614174000
Content-Type: application/json
```

---

## Implementation

Request tracing is implemented using middleware in the API layer.

The middleware:

- Checks for the presence of the `Request-Id` header
- Generates a new GUID if one is not provided
- Attaches the request ID to the current request context
- Ensures the same ID is included in the response headers

Because middleware wraps the request pipeline, the same request ID is available:

- At the start of the request
- Throughout application processing
- When generating the response

This enables consistent tracing across all layers of the application.

---

## Why Request Tracing?

Request tracing provides a consistent way to follow a request across the system.

It is particularly useful for:

- Debugging issues by linking all logs for a request
- Identifying performance bottlenecks using request duration
- Log correlation (see [Logging](logging.md))
- Supporting future observability tooling (e.g., distributed tracing)

Request tracing becomes increasingly important as systems grow in complexity.

---

## Relationship to Request Flow

Request tracing is integrated at the middleware level and spans the entire request lifecycle.

See [Request Flow](request-flow.md) for a detailed breakdown of how requests move through the system.

---

- [Back to top](#request-tracing)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)