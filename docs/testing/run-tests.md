[Docs](docs/index.md) / [Testing](docs/testing/index.md) / [Run Tests](run-tests.md)

# Run Tests

---

This guide explains how to run the test suite locally.

The project uses **xUnit** for testing and **Coverlet** for code coverage.

___

## Contents

- [Quick Start](#quick-start) - Run tests using project scripts
- [Manual Commands](#manual-commands) - Run tests using dotnet CLI
- [Custom Scripts](#custom-scripts) - Details about test automation scripts
- [Notes](#notes) - Additional information

---

## Quick Start

### Run Tests

Runs all tests using the project script:

```powershell
.\scripts\run-coverage.ps1
```

### Run Tests with Coverage

Runs tests, generates coverage, and opens the report:

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
dotnet test tests/PortfolioWebApp.Tests
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
- Will be extended in the future to include additional test types (e.g., integration tests)

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
- [Back to Testing](docs/testing/index.md)
- [Back to Docs Home](docs/index.md)