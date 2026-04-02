[Docs](docs/index.md) / [Testing](docs/testing/index.md) / [CI](ci.md)

# Continuous Integration (CI)

---

The project uses **GitHub Actions** to automatically run tests and validate code changes.

CI
status: [![.NET CI](https://github.com/JamesDirr1/PortfolioWebApp/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/JamesDirr1/PortfolioWebApp/actions/workflows/dotnet-test.yml)

Continuous Integration (CI) ensures that the application remains stable and that new changes do not introduce
regressions.

---

## Contents

- [Workflow](#workflow) – CI triggers and workflow file location
- [What CI Does](#what-ci-does) – Build, test, and coverage steps executed in CI
- [Viewing CI Results](#viewing-ci-results) – Access logs, test results, and coverage output
- [Artifacts](#artifacts) – Downloadable test results and coverage reports

---

## Workflow

The CI pipeline runs automatically when:

- Code is pushed to:
- `main`
- `version/*`
- A pull request targets:
- `main`
- `version/*`

The CI pipeline is defined in:

```
.github/workflows/dotnet-test.yml
```

---

## What CI Does

The CI workflow performs the following steps:

1. **Checkout Repository**
    - Checks out the repository code
2. **Setup .NET**
    - Installs .NET SDK (`10.0.x`)
3. **Cache Dependencies**
    - Caches NuGet packages to improve build performance
4. **Restore**
    - Restores dependencies using:
   ```bash
   dotnet restore ./PortfolioWebApp.slnx
   ```
5. **Build**
    - Builds the solution in Release mode:
   ```bash
   dotnet build ./PortfolioWebApp.slnx --no-restore --configuration Release
   ```
6. **Run Tests**
    - Executes all tests with coverage enabled:
   ```bash
   dotnet test ./PortfolioWebApp.slnx --no-build --configuration Release --collect:"XPlat Code Coverage" --logger "trx" --results-directory ./tests/TestResults
   ```
7. **Generate Coverage Report**
    - Uses ReportGenerator to create coverage reports:
        - HTML report
        - GitHub summary
8. **Upload Artifacts**
    - Test results (`tests/TestResults`)
    - Coverage report (`tests/coverage-report`)
    - Retained for 7 days
9. **Publish Coverage Summary**
    - Adds coverage summary directly to the GitHub Actions job output

---

## Viewing CI Results

To view CI results:

1. Go to the GitHub repository
2. Click on the **Actions** tab
3. Select the latest workflow run

From there you can:

- View test results (`.trx`)
- Download coverage reports
- Inspect logs
- Debug failures

---

## Artifacts

The CI pipeline uploads the following artifacts:

- **test-results**
    - Raw test output files
- **coverage-report**
    - Generated coverage reports (HTML + summary)

Artifacts are retained for **7 days**.

---

- [Back to top](#run-tests)
- [Back to Testing](docs/testing/index.md)
- [Back to Docs Home](docs/index.md)
