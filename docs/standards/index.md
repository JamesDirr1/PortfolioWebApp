[Docs](../index.md) / [Standards](./index.md)

# Standards

---

This document defines development standards for the project, including naming conventions, branching strategy, and
general best practices.

---

## Contents

- [Git](#git) - Branching strategy, commit message conventions, and pull request guidelines.
- [Development Workflow](#development-workflow) - Overview of the development process, testing, and deployment
  practices.

---

## Git

Standards around using Git for version control, including branching strategy, commit message conventions, and pull
request guidelines.

### Branching Strategy

- `main`
    - Stable branch containing production-ready code

- `version/x.y.z`
    - Used for development of a specific version
    - Created from `main`
    - Merged back into `main` when the version is finalized

  Version format:
    - `x` (major) – breaking changes (e.g., 1.0.0 initial release)
    - `y` (minor) – new features
    - `z` (patch) – bug fixes

  Example:
    - `version/1.1.0` – new features
    - `version/1.0.1` – bug fixes

- `type/short-description`
    - Feature branches created from a `version/*` branch
    - Merged back into the same version branch

| Type     | Description                                 |
|----------|---------------------------------------------|
| feature  | New functionality                           |
| bugfix   | Fixing a bug                                |
| refactor | Code improvements without changing behavior |
| docs     | Documentation changes                       |
| test     | Adding or updating tests                    |

---

### Branch Flow

Typical workflow:

1. Create a feature branch from a `version/*` branch
2. Implement changes
3. Merge into the version branch
4. Merge version branch into `main` when released

---

### Commit Message Conventions

Commit messages should follow the format:

```
type: short description
```

Examples:

feature: add categories endpoint
bugfix: fix null category response
docs: update API documentation

---

Pull Request Guidelines

Before opening a pull request:

- Ensure all tests pass
- Ensure new code includes tests where applicable
- Ensure documentation is updated if needed
- Ensure code is properly formatted
- Provide a clear description of the changes

---

## Development Workflow

Typical workflow:

- Pull latest changes from main
- Create a new branch (feature/...)
- Implement changes
- Add or update tests
- Run tests locally
- Open a pull request
- CI validates the changes

---

- [Back to top](#standards)
- [Back to Docs Home](../index.md)
