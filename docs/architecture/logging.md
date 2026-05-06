[Docs](../index.md) / [Architecture](./index.md) / [Logging](logging.md)

# Logging

---

This application uses **Serilog** for structured logging.

Logging is configured with multiple sinks to support different use cases:

- **Console output** for human-readable logs during development
- **File output (JSON)** for structured logging and analysis

Logging also supports request correlation via the `Request-Id` header.

___

## Serilog

This project uses **Serilog** integrated with ASP.NET Core.

Serilog provides:

- Structured logging (key-value based logs)
- Flexible output sinks (console, files, etc.)
- Improved log readability and filtering

___

## Logging Architecture

Logging is implemented using a combination of:

- ASP.NET Core middleware
- Serilog enrichment
- Structured logging patterns

### Middleware Responsibilities

A custom middleware (`RequestContextLoggingMiddleware`) handles:

- Request start logging
- Request end logging
- Request ID generation and propagation
- Log enrichment with request context

This ensures consistent logging behavior across the entire request lifecycle.

---

## Request Correlation

Logging is integrated with request tracing using the `Request-Id` header.

- Each request is associated with a unique request ID
- The same ID is included in all logs for that request
- The ID is also returned in the response headers

This enables end-to-end tracing of requests across the system.

---

### Request Lifecycle Logging

### Request Start

Logs request metadata:

- HTTP method
- Request path
- Request ID

### Request End

Logs response metadata:

- HTTP status code
- Elapsed time
- Request ID

### Log Levels

Different log levels are used based on response status:

- **Information** → Successful requests (2xx)
- **Warning** → Client errors (4xx)
- **Error** → Server errors (5xx)

This allows logs to be filtered effectively based on severity.

---

## Structured Logging

Logs are written in a structured format to support querying and analysis.

Instead of plain text logs, key data is captured as structured fields:

- Request ID
- HTTP method
- Path
- Status code
- Duration

This enables:

- Searching logs by request ID
- Filtering by status code or endpoint
- Easier debugging and monitoring

---

## Sinks

### Console

The console sink provides a human-readable format for logs.

It includes:

- Timestamp
- Log Level
- Message
- Request ID

#### Example

> [2026-03-24 21:52:39 INF] Received request (method: GET) (path: /api/Categories) (req:
> 9a0aa425-38f0-498c-9510-80d3a50fb1dc)

### File

The file sink writes logs in **JSON format** for structured logging.

This enables:

- Key-value searching
- Better filtering and analysis
- Improved request correlation

This log format includes:

- Timestamp
- Event ID
- Category
- Log level
- Message
- Additional request data

#### Example

```json 
{
  "Timestamp": "2026-03-24 21:52:39",
  "EventId": 0,
  "Category": "PortfolioWebApp.Api.Middleware.RequestContextLoggingMiddleware",
  "LogLevel": "Information",
  "Message": "Received request",
  "Request": {
    "Request-Id": "9a0aa425-38f0-498c-9510-80d3a50fb1dc",
    "Method": "GET",
    "Path": "/api/Categories"
  }
}
```

---

## Integration with Request Flow

Logging is tightly integrated with the request pipeline.

See:

- [Request Flow](request-flow.md)
- [Request Tracing](request-tracing.md)

___

- [Back to top](#Logging)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)