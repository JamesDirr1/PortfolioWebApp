[Docs](../index.md) / [Architecture](./index.md) / [Design Decisions](decisions.md)

# Design Decisions

---

## Contents

- [Why make this project?](#why-make-this-project)
    - [Why not just modify the original project?](#why-not-just-modify-the-original-project)
- [Why ASP.NET Core?](#why-aspnet-core)
- [Why PostgreSQL?](#why-postgresql)
- [Why layered architecture?](#why-layered-architecture)

--- 

## Why make this project?

To learn and practice modern backend development in C# using ASP.NET Core, Entity Framework Core, and PostgreSQL, while
applying layered architecture principles.

I wanted to create a real-world project that could serve as a portfolio piece and demonstrate my skills in backend
development, API design, and software architecture.

### Why not just modify the original project?

The original project was built with Flask and MySQL without a focus on scalability or maintainability.
Modifying it would have been constrained by earlier design decisions and would not have allowed me to apply best
practices from the ground up.
Rebuilding it gave me the opportunity to create a more structured, maintainable, and scalable application while learning
new technologies.

---

## Why ASP.NET Core

Chosen to learn modern backend development in C# to align with my career goals and explore a new tech stack.

---

## Why PostgreSQL

A reliable relational database with strong EF Core support.

It also provides experience with a different database system beyond my previous work with MySQL and MS SQL.

---

## Why layered architecture

Improves maintainability and testability by separating concerns and allowing for future scalability.
It also allows for better organization of code.

---

- [Back to top](#design-decisions)
- [Back to Architecture Documentation](./index.md)
- [Back to Docs Home](../index.md)
