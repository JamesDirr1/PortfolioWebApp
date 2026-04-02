[Docs](../index.md) / [Architecture](docs/architecture/index.md) / [Request Flow](request-flow.md)

# Request Flow

---

This document describes how a request flows through the application, from the client to the database and back.

---

## Flow  
  
A typical request follows this path:  
  
> Client → Middleware (request start) → Controller → Service → Repository → DbContext → PostgreSQL → Middleware (response end) → Client

---

## Step-by-Step Example (Categories)  
  
### 1. Client Request  
  
The client sends a request:

>GET /api/categories

### 2. Middleware  (Request Start)
  
Middleware processes the request before it reaches the controller.  
  
This includes:  
  
- Generating or reading the `Request-Id`  
- Logging request details using Serilog

### 3. Controller (API Layer)  
  
- Receives the HTTP request  
- Validates input (if applicable)  
- Calls the appropriate Application service

### 4. Application Service  
  
- Contains business logic  
- Coordinates data retrieval  
- Calls the repository layer

### 5. Repository (Infrastructure Layer)  
  
- Uses EF Core to interact with the database  
- Executes queries against PostgreSQL

### 6. DbContext / Database  
  
- EF Core translates queries into SQL  
- PostgreSQL executes the query  
- Data is returned to the application

### 7. Response Mapping  
  
- Data is mapped to DTOs  
- Ensures only intended fields are returned

### 8. Middleware (Response End)  
  
After the downstream pipeline completes, control returns to the middleware.  
  
This allows the middleware to:  
  
- Read the final response status code  
- Stop the timer and calculate elapsed time  
- Log completion details for the request  
- Ensure the `Request-Id` is included in the response headers

### 9. Response  
  
- The completed HTTP response is returned to the client

---

- [Back to top](#request-flow)
- [Back to Architecture Documentation](docs/architecture/index.md)
- [Back to Docs Home](../index.md)