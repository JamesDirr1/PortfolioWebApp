[Docs](../index.md) / [Testing](./index.md) / [Strategy](strategy.md)

# Strategy

---

The project focuses on testing core application logic to ensure correctness and maintainability.

The project uses xUnit for unit testing and Coverlet for code coverage.

This approach keeps tests focused, fast, and maintainable.

---

## Content

- [Current Scope](#current-scope)
- [Philosophy](#philosophy)
- [What is Tested](#what-is-tested)
- [Continuous Integration](#continuous-integration)

---

## Current Scope

- Unit tests for application services
- Validation of business logic
- Testing of edge cases and failure scenarios

### Not Covered (Yet)

- Integration test
- End-to-end tests

---

## Philosophy

- All tests should pass before code is merged or pushed to a working branch
- New functionality should include corresponding unit tests
- Code coverage should be high (target ~90%), but should not prioritize coverage over meaningful tests
- Testing should focus on business logic and application behavior rather than framework internals

---

## What is Tested

The following areas are covered by tests:

- Application services
- Domain logic
- Validation rules
- Custom request tracing and logging

The following are not directly tested

- Framework behavior (ASP.NET routing, EF Core internals)
- Simple data models without logic

---

## Continuous Integration

Tests are executed automatically using GitHub Actions.  
See [CI](ci.md) for more information.

This ensures:

- New changes do not break existing functionality
- Code remains stable over time

---

- [Back to top](#strategy)
- [Back to Testing](./index.md)
- [Back to Docs Home](../index.md)
