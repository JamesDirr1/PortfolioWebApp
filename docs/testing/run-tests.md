[Docs](../index.md) / [Testing](./index.md) / [Run Tests](run-tests.md)

# Run Tests

---

This guide explains how to run the test suite locally.

The project uses:

- **xUnit** for testing
- **Moq** for mocking
- **FluentAssertions** for assertions
- **Coverlet** for code coverage

___



## Contents

- [Prerequisites](#prerequisites) - Required tools and setup
- [Quick Start](#quick-start) - Run tests using project scripts
- [Manual Commands](#manual-commands) - Run tests using dotnet CLI
- [Custom Scripts](#custom-scripts) - Details about test automation scripts
- [Notes](#notes) - Additional information

---

## Prerequisites

Some infrastructure tests require **Docker**.

The Infrastructure test project uses Testcontainers to automatically start a temporary PostgreSQL container during the test run.

You do not need to manually run `docker compose up` before running tests.

Required:

- Docker installed
- Docker Desktop running

---

## Quick Start

### Run Tests with Coverage (Recommended)

Runs all tests, generates coverage, and opens the report:

```powershell
.\scripts\run-coverage.ps1
```

---

## Manual Commands

### Run All Tests

```bash  
dotnet test
```

This will:

- Build the solution
- Run all tests
- Output results to the console

### Run Tests for a Specific Project

```bash
dotnet test tests/PortfolioWebApp.Api.Tests
dotnet test tests/PortfolioWebApp.Application.Tests
dotnet test tests/PortfolioWebApp.Infrastructure.Tests
```

### Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Coverage reports will be generated in:

```
TestResults/
```

### Watch Mode (Optional)

To automatically re-run tests when files change:

```bash
dotnet watch test
```

### Test Output

After running tests, you will see:

- Total tests run
- Passed / failed tests
- Execution time

Example:

```bash
Passed! 10 tests run, 0 failed, 0 skipped
```

---

## Custom Scripts

Custom scripts are provided to simplify common testing workflows.

These are located in `/scripts`

### Run Tests

Runs all tests in the project.

```bash
.\scripts\run-test.ps1
```

This script:

- Executes all unit tests

### Run Test with Coverage

Generates a fresh coverage report.

```bash
.\scripts\run-coverage.ps1
```

This script:

- Clears old test results and coverage reports
- Runs all unit tests
- Generates a coverage report
- Opens the coverage report automatically

This is the recommended way to run tests when working with coverage.

___

## Notes

- Tests are also executed automatically in CI (see [CI](ci.md))
- Focus is on testing application logic and business rules (See [Strategy](strategy.md))

---

- [Back to top](#run-tests)
- [Back to Testing](./index.md)
- [Back to Docs Home](../index.md)
