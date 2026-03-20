[Docs](../index.md) / [Architecture](index.md) / [Request Flow](request-flow.md)
# Request Flow

---

A typical request flows through the system as follows:

`Client → API Controller → Application Service → Repository → Database`

## Example Flow (Categories)

1. Client sends GET /api/categories
2. Controller receives the request
3. Controller calls a service in the Application layer
4. Service retrieves data via a repository
5. Repository queries PostgreSQL via EF Core
6. Data is returned and mapped to DTOs
7. Response is returned to the client

---

- [Back to top](#request-flow)
- [Back to Architecture Documentation](index.md)
- [Back to Docs Home](../index.md)