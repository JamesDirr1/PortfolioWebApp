[Docs](../index.md) / [Architecture](./index.md)

# Application Architecture

---

This application is a backend API built using **ASP.NET Core Web API**, **Entity Framework Core**, and **PostgreSQL**.

It follows a layered architecture to separate concerns, improve maintainability, and support scalability.

It is designed to support an artist portfolio platform where categories, projects, and artwork can be managed through a
RESTful API.

The project currently focuses on:

- Building a structured RESTful API
- Using PostgreSQL for persistent data storage
- Applying layered architecture and separation of concerns
- Implementing automated testing and CI workflows

---

## Contents

- [Project Structure](project-structure.md) – Overview of solution layout and projects
- [Request Flow](request-flow.md) – How requests move through the system
- [Design Decisions](decisions.md) – Key architectural choices and reasoning
- [Data Access](data-access.md) – Database and EF Core usage
- [Request Tracing](request-tracing.md) – Request ID handling and observability
- [Logging](logging.md) - Structure of logging

---
- [Back to top](#application-architecture)
- [Back to Docs Home](../index.md)
