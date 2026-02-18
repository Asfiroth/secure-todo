# Secure Todo

A modern, secure todo application built with .NET 10 and React 19, following Clean Architecture principles.

## Tech Stack

### Backend (API)

- **.NET 10.0** - Latest .NET framework
- **C#** with latest language version
- **ASP.NET Core** - Web API framework
- **Clean Architecture** - Separation of concerns with Domain, Application, Infrastructure, and API layers

#### Key Libraries

- **Entity Framework Core 10.0.3** - ORM with SQL Server provider
- **Mediator 3.0.1** - Source-generated CQRS pattern implementation
- **FluentValidation 12.1.1** - Request validation
- **Ardalis.Result 10.1.0** - Result pattern for API responses
- **Serilog 10.0.0** - Structured logging with enrichers (ClientInfo, Environment, Thread)
- **Scalar 2.12.41** - API documentation (available at `/docs` in development)
- **JWT Bearer Authentication** - Token-based auth via Keycloak OIDC

### Frontend (App)

- **React 19.2.0** - UI library
- **React Router 7.13.0** - File-based routing framework (SSR disabled, client-only SPA)
- **TypeScript 5.9.3** - Type-safe JavaScript (strict mode)
- **Vite 7.3.1** - Build tool and dev server
- **Tailwind CSS 4.1.18** - Utility-first CSS framework

#### Key Libraries

- **Axios 1.13.5** - HTTP client for API communication
- **Keycloak JS 26.2.3** - Keycloak JavaScript adapter for OIDC authentication
- **React Hook Form 7.71.1** - Form state management and validation
- **React Hot Toast 2.6.0** - Toast notifications
- **Heroicons 2.2.0** - SVG icon library
- **Lodash 4.17.23** - Utility functions

#### UI Components

Reusable component library with dedicated CSS files using Tailwind `@apply`:

- **Card** - Content container
- **Header** - Page header
- **Label** - Form labels
- **Loader** - Loading indicator with bubble animation
- **Modal** - Accessible dialog with keyboard support and backdrop
- **SegmentedControl** - Toggle/filter control
- **SelectField** - Dropdown select input
- **TextAreaField** - Multi-line text input
- **TextField** - Single-line text input
- **TextError** - Validation error display
- **TodoFormModal** - Todo creation/editing modal form

### Database

- **SQL Server 2025** (latest) - Application database running in Docker container
- **PostgreSQL 17** - Keycloak database running in Docker container

### Authentication

- **Keycloak 26.0.5** - Open Source Identity and Access Management
  - OpenID Connect (OIDC) provider
  - Authorization Code flow with PKCE (S256)
  - Single Sign-On (SSO) capabilities
  - User management and role-based access

### DevOps & Tooling

- **Docker** & **Docker Compose** - Containerization and orchestration
- **Nginx** (unprivileged) - Production SPA serving
- **ESLint 9** - JavaScript/TypeScript linting
- **Prettier 3** - Code formatting (with Tailwind plugin)
- **EF Core Migrations** - Database schema management via shell scripts
- **Architecture Tests** - ArchUnitNET + TUnit for enforcing Clean Architecture constraints
- **Target OS**: Linux (ARM64)

## Project Structure

```
secure-todo/
├── api/                              # Backend .NET application
│   ├── SecureTodo.Api/              # API layer (endpoints, auth service)
│   ├── SecureTodo.Application/      # Application layer (use cases, CQRS, validators)
│   ├── SecureTodo.Domain/           # Domain layer (entities, value objects, enums)
│   ├── SecureTodo.Infrastructure/   # Infrastructure layer (EF Core, repositories, query services)
│   ├── SecureTodo.ArchitectureTests/# Architecture constraint tests
│   ├── Directory.Build.props        # Shared build configuration
│   └── Directory.Packages.props     # Centralized package management
├── app/                             # Frontend React application
│   ├── src/
│   │   ├── components/             # Reusable UI components
│   │   ├── contexts/               # React contexts (Auth)
│   │   ├── hoc/                    # Higher-order components (AuthProvider, ProtectedRoute)
│   │   ├── hooks/                  # Custom hooks (useAuth, useTodo)
│   │   ├── layouts/                # Page layouts (Authorized, Guest)
│   │   ├── lib/                    # Library setup (Keycloak, Axios API client)
│   │   ├── routes/                 # Route pages (login, todos)
│   │   ├── types/                  # TypeScript type definitions
│   │   └── utils/                  # Utility helpers
│   ├── package.json                # Node dependencies
│   ├── vite.config.ts              # Vite configuration
│   └── react-router.config.ts      # React Router configuration
├── .docker/                         # Docker build files
│   ├── api.dockerfile              # .NET API multi-stage build
│   ├── app.dockerfile              # React SPA multi-stage build (Nginx)
│   └── keycloak.dockerfile         # Keycloak with import directory
├── keycloak/                        # Keycloak configuration
│   └── import/
│       └── todo-realm.json         # Todo realm with client, roles, and users
├── scripts/                         # Utility scripts
│   ├── add_migration.sh            # Create EF Core migration
│   ├── bundle_migration.sh         # Generate migration bundle
│   └── execute_bundle.sh           # Run migration bundle
└── docker-compose.yml               # Docker services configuration
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js (LTS) and npm
- Docker and Docker Compose

### Running the Services

```bash
docker-compose up -d
```

This will start:
- **SQL Server 2025** on port 1433 (application database)
- **PostgreSQL 17** on port 5432 (Keycloak database)
- **Keycloak 26.0.5** on port 8180 (authentication server)

The Keycloak realm `todo` will be automatically imported with:
- Client: `todo-app` (React SPA)
- Test User: `testuser` / `password123`

### Running the API

```bash
cd api/SecureTodo.Api
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5208
- HTTPS: https://localhost:7251
- API Docs: http://localhost:5208/docs (development only)

### Running the Frontend

```bash
cd app
npm install
npm run dev
```

### Running Architecture Tests

```bash
cd api
dotnet test
```

## Development

### Frontend

- `npm run dev` - Start development server with hot reload
- `npm run build` - Build for production
- `npm run lint` - Run ESLint with auto-fix
- `npm run lint:format` - Format code with Prettier

### Backend

The API follows Clean Architecture principles:

- **Domain Layer**: Core business entities, value objects, and enums (no external dependencies)
- **Application Layer**: Use cases (CQRS handlers), validators, service interfaces
- **Infrastructure Layer**: EF Core DbContext, repositories, query services, interceptors
- **API Layer**: Minimal API endpoints, authentication service, DI configuration

### Database Migrations

```bash
# Create a new migration
./scripts/add_migration.sh -n <MigrationName>

# Generate a migration bundle
./scripts/bundle_migration.sh

# Execute the migration bundle
./scripts/execute_bundle.sh
```

## Architecture Patterns

- **Clean Architecture** - Dependency inversion and separation of concerns
- **CQRS** - Command Query Responsibility Segregation via source-generated Mediator
- **Result Pattern** - Explicit success/failure handling with Ardalis.Result
- **Repository Pattern** - Data access abstraction (write-side)
- **Query Service Pattern** - Data access for read operations
- **Strongly-Typed IDs** - Domain entity IDs as value objects

## Authentication & Authorization

The application uses **Keycloak** for authentication and authorization with OpenID Connect (OIDC).

### Keycloak Configuration

**Realm:** `todo`

**Client Configuration:**
- Client ID: `todo-app`
- Client Type: Public (React SPA)
- Protocol: OpenID Connect
- Flow: Authorization Code with PKCE (S256)
- Redirect URIs:
  - `http://localhost:3000/*`
  - `http://localhost:5173/*`
  - `http://localhost:8080/*`

**Roles:**
- `user` - Basic user role (default)
- `admin` - Administrator role

**Security Features:**
- PKCE (Proof Key for Code Exchange) enabled
- Brute force protection enabled
- Password reset allowed
- Email verification supported

### Accessing Keycloak

**Admin Console:**
- URL: http://localhost:8180/admin
- Username: `admin`
- Password: `admin`

**Todo Realm:**
- URL: http://localhost:8180/realms/todo
- Account Console: http://localhost:8180/realms/todo/account
- OpenID Configuration: http://localhost:8180/realms/todo/.well-known/openid-configuration

**Test User:**
- Username: `testuser`
- Email: `testuser@example.com`
- Password: `password123`
