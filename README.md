# my-portfolio-mvc

A personal portfolio built with ASP.NET Core MVC, a separate ASP.NET Core Web API, PostgreSQL, and Docker. The MVC project handles the frontend and communicates with the API through typed HTTP clients, while the API manages data, authentication, and backend operations. Each part runs in its own container and is managed with Docker Compose.

## About this project
This portfolio is structured as a small full‑stack application using Microsoft technologies.
The MVC frontend is responsible for the UI, routing, and user interactions. It sends requests to the Web API, which contains the business logic, data access, and security features.

The API uses Entity Framework Core with PostgreSQL, ASP.NET Core Identity for user management, JWT authentication with refresh tokens, and additional request‑handling features such as rate limiting and CORS policies. This keeps the backend consistent with common patterns used in modern .NET applications.

The project originally used SQL Server before being migrated to PostgreSQL. This provided practical experience adapting an existing data layer to a different relational database system.

## Infrastructure
The application is fully containerized.
The MVC app, API, and PostgreSQL database each run in their own Docker containers, orchestrated with Docker Compose. This setup keeps the environment consistent and makes the project easy to run locally or deploy.

## Development workflow
The project is developed using GitHub for version control, branches, and pull requests.
A simple CI/CD workflow is used to keep updates organized and aligned with modern software engineering practices. Changes are pushed to GitHub and deployed through Railway, which hosts the containerized services without requiring manual server setup.

This workflow keeps the project easy to maintain and provides a practical way to work with automated builds and deployments.

## What this project demonstrates
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker & Docker Compose
- ASP.NET Core Identity
- JWT authentication & refresh tokens
- Typed HTTP clients
- Rate limiting & CORS policies
- Cloudflare Turnstile
- CI/CD using GitHub + Railway
- Containerized hosting

## Repository
This repository contains the MVC portion of the project, along with workflow files and deployment history visible in GitHub Actions and Railway deployments.
Deployment logs and pull requests provide a simple look at how the project is updated and maintained. 
