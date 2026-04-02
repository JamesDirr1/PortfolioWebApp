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

## Request Correlation  
  
Logging is integrated with request tracing using the `Request-Id` header.

- Each request is associated with a unique request ID
- The same ID is included in all logs for that request
- The ID is also returned in the response headers
- This enables end-to-end tracing of requests

Request correlation and logging enrichment are handled through middleware.

This ensures that:

- Every request is assigned a request ID
- The request ID is consistently included in all logs
- Logging remains consistent across the application

### Request Lifecycle Logging

Logging occurs at two points in the middleware pipeline:

- **Request start** - logs request metadata such as method, path, and request ID
- **Request end** - logs response metadata such as status code and elapsed time

See [Request Tracing](request-tracing.md) and [Request Flow](request-flow.md) for more details.

---

## Sinks

### Console 

The console sink provides a human-readable format for logs.

It includes important information such as:

- Timestamp 
- Log Level
- Message 
- Request ID

#### Example

> [2026-03-24 21:52:39 INF] Received request (method: GET) (path: /api/Categories) (req: 9a0aa425-38f0-498c-9510-80d3a50fb1dc)

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
{"Timestamp":"2026-03-24 21:52:39",
"EventId":0,
"Category":"PortfolioWebApp.Api.Middleware.RequestContextLoggingMiddleware",
"LogLevel":"Information",
"Message":"Received request",
"Request":{
	"Request-Id":"9a0aa425-38f0-498c-9510-80d3a50fb1dc",
	"Method":"GET",
	"Path":"/api/Categories"
	}
}
```

___
- [Back to top](#Logging)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)