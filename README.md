# Portfolio Web App (ASP.NET Core + PostgreSQL)

![.NET](https://img.shields.io/badge/.NET-10-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-blue)
[![.NET CI](https://github.com/JamesDirr1/PortfolioWebApp/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/JamesDirr1/PortfolioWebApp/actions/workflows/dotnet-test.yml)

---

## Overview

This project is a portfolio web application backend built with **ASP.NET Core Web API**, **Entity Framework Core**, and **PostgreSQL**.

It is designed to support an **artist portfolio platform** where artwork can be uploaded and managed, while also serving as a technical portfolio project to showcase my backend development skills.

The project currently focuses on:

- Building a structured RESTful API
- Using PostgreSQL for persistent data storage
- Applying layered architecture and separation of concerns
- Implementing automated testing and CI workflows

---

## Tech Stack

- **Backend:** ASP.NET Core Web API
- **Language:** C#
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Testing:** xUnit
- **CI/CD:** GitHub Actions

---

## Project Evolution

This project is a complete redesign of my original portfolio application:

- Original Project: [Kelsey's Portfolio Website](https://github.com/JamesDirr1/Kelseys_Portfolio_Website)

### Version 1 (Original Portfolio)
- Built with Flask (Python) and MySQL
- Not designed for scalability or maintainability
    - Tightly coupled codebase
    - Non API-driven design
    - No ORM
- Focused on getting a basic portfolio up and running
- First iteration to learn web development and backend concepts

### Version 2 (Current Project)
- Rebuilt using **ASP.NET Core Web API**
- Introduced **PostgreSQL** for persistent data storage
- Implemented a **layered architecture**:
    - API
    - Application
    - Domain
    - Infrastructure
- Added **unit testing and CI pipelines**
- Designed for real-world usage (artist content management)
- Improved separation of concerns and maintainability

### Why I Rebuilt It

I chose to rebuild this project instead of modifying the original so I could:

- Apply best practices from the ground up
- Avoid being constrained by earlier design decisions
- Focus on modern backend development patterns
- Align the project with my long-term career goals

---

## Architecture

This project follows a layered architecture to improve maintainability and testability:

- **API Layer** → Handles HTTP requests and routing
- **Application Layer** → Business logic and services
- **Domain Layer** → Core entities and models
- **Infrastructure Layer** → Database access and external dependencies

This separation allows for better scalability, easier testing, and cleaner code organization.

---

## Features

- RESTful API for managing portfolio content
- PostgreSQL integration using Entity Framework Core
- Layered architecture for clean separation of concerns
- Unit testing for core application logic
- Continuous Integration with GitHub Actions
- Code coverage tracking

---

## Getting Started

### Prerequisites

- .NET SDK 10
- PostgreSQL (or Docker)

### Run the Application

```bash
dotnet restore
dotnet build
dotnet run --project src/backend/PortfolioWebApp.Api