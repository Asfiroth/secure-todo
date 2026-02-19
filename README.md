# Secure Task Manager

This example showcases a simple yet secure application built with .NET 10 and React 19, following Clean Architecture principles.

## Tech Stack

### Backend (API)

- **.NET 10.0** - Latest .NET framework
- **C#** with latest language version
- **ASP.NET Core** - Web API framework
- **Clean Architecture** - Separation of concerns with Domain, Application, Infrastructure, and API layers

### Frontend (App)

- **React 19.2.0** - UI library
- **React Router 7.13.0** - Route-config based routing with nested layouts (SSR disabled, client-only SPA)
- **TypeScript 5.9.3** - Type-safe JavaScript (strict mode)
- **Vite 7.3.1** - Build tool and dev server
- **Tailwind CSS 4.1.18** - Utility-first CSS framework

### Database

- **SQL Server 2025** (latest) - Application database running in Docker container
- **PostgreSQL 17** - Keycloak database running in Docker container

### Authentication

- **Keycloak 26.3.4** - Open Source Identity and Access Management
  - OpenID Connect (OIDC) provider
  - Authorization Code flow with PKCE (S256)
  - Single Sign-On (SSO) with silent check via iframe
  - User management and role-based access

### DevOps & Tooling

- **Docker** & **Docker Compose** - Containerization and orchestration
- **Nginx** - Reverse proxy (`*.localhost` subdomain routing) and SPA serving
- **ESLint 9** - JavaScript/TypeScript linting
- **Prettier 3** - Code formatting (with Tailwind plugin)
- **EF Core Migrations** - Database schema management via shell scripts
- **Architecture Tests** - ArchUnitNET + TUnit for enforcing Clean Architecture constraints
- **Target OS**: Linux

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
│   ├── public/
│   │   └── silent-check-sso.html   # Keycloak silent SSO check callback
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
│   ├── keycloak.dockerfile         # Keycloak with realm import
│   └── migrator.dockerfile         # EF Core migration runner
├── proxy/
│   └── proxy.conf                  # Nginx reverse proxy configuration
├── keycloak/                        # Keycloak configuration
│   └── import/
│       └── todo-realm.json         # Todo realm with client, roles, and users
├── scripts/                         # Utility scripts
│   ├── add_migration.sh            # Create EF Core migration
│   ├── bundle_migration.sh         # Generate migration bundle
│   ├── execute_bundle.sh           # Run migration bundle
│   └── init-db.sql                 # Database initialization script
└── docker-compose.yml               # Docker services configuration
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js (LTS) and npm
- Docker and Docker Compose

### Running with Docker

#### 1. Configure local DNS (if needed)

Most modern browsers (Chrome, Edge) resolve `*.localhost` to `127.0.0.1` automatically. If your browser or system tools don't, add these entries to your `/etc/hosts`:

```
127.0.0.1  app.localhost api.localhost auth.localhost
```

#### 2. Start all services

```bash
docker-compose up -d
```

This will start:
- **SQL Server 2025** on port 1433 (application database)
- **PostgreSQL 17** on port 5432 (Keycloak database)
- **Keycloak 26.3.4** (authentication server)
- **.NET API** (backend)
- **React SPA** (frontend)
- **Nginx Reverse Proxy** on port 80 (routes `*.localhost` subdomains to services)

The database is initialized via `scripts/init-db.sql` and migrations are applied automatically by the `migrator` service before the API starts.

#### Accessing Services

| Service | URL |
|---------|-----|
| React SPA | http://app.localhost |
| .NET API | http://api.localhost |
| API Docs | http://api.localhost/docs |
| Keycloak Admin | http://auth.localhost/admin |
| Keycloak Realm | http://auth.localhost/realms/todo |

The Keycloak realm `todo` is automatically imported with:
- Client: `todo-app` (React SPA)
- Test User: `testuser` / `password123`

### Running Locally (without Docker)

For local development, the API and frontend run directly on the host.

#### Local Configuration Files

The following files are required for local development and are intentionally ignored by git:

- `app/.env`
- `api/SecureTodo.Api/appsettings.Development.json`

Create them locally with values matching your environment.

**Frontend: `app/.env`**

```env
VITE_TODO_API=http://localhost:5208
VITE_KEYCLOAK_URL=http://localhost:8080
VITE_KEYCLOAK_REALM=todo
VITE_KEYCLOAK_CLIENT_ID=todo-app
```

**Backend: `api/SecureTodo.Api/appsettings.Development.json`**

```json
{
  "ConnectionStrings": {
    "TodoDb": "Server=localhost,1433;Database=todo-db;User ID=sa;Password=<your-password>;TrustServerCertificate=True;Encrypt=True;"
  },
  "Identity": {
    "Authority": "http://localhost:8080/realms/todo",
    "Issuer": "http://localhost:8080/realms/todo",
    "Audiences": "account"
  },
  "CrossOrigins": "http://localhost:5173"
}
```
> NOTE: keep real credentials only in local files or a secret manager. Do not commit secrets.

#### Running the API

```bash
cd api/SecureTodo.Api
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5208
- HTTPS: https://localhost:7251
- API Docs: http://localhost:5208/docs (development only)

#### Running the Frontend

```bash
cd app
npm install
npm run dev
```

## Development Workflows

### Frontend

- `npm run build` - Build for production
- `npm run lint` - Run ESLint with auto-fix
- `npm run lint:format` - Format code with Prettier

### Backend

#### Architecture Tests

```bash
cd api
dotnet test
```

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

### Backend

- **Clean Architecture** - Dependency inversion and separation of concerns
- **CQRS** - Command Query Responsibility Segregation via source-generated Mediator
- **Result Pattern** - Explicit success/failure handling with Ardalis.Result
- **Repository Pattern** - Data access abstraction (write-side)
- **Query Service Pattern** - Data access for read operations
- **Strongly-Typed IDs** - Domain entity IDs as value objects

### Frontend

- **Route-Based Layout Composition** - Separate guest and authorized app shells via React Router layouts
- **Authentication Boundary Pattern** - Session state and auth actions centralized in `AuthProvider` + `ProtectedRoute`
- **Custom Hook Service Pattern** - Domain-specific operations encapsulated in `useAuth` and `useTodo`
- **HTTP Client Abstraction** - Shared Axios client with auth token injection and 401 token refresh retry
- **Component Composition Pattern** - Reusable UI primitives composed into route views and feature components
- **Type-Driven UI Contracts** - Shared TypeScript models for auth and todo data across hooks and components

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
  - `http://app.localhost/*` (Docker deployment)
  - `http://localhost:5173/*` (local development)
  - `http://localhost:9001/*`

**Roles:**
- `user` - Basic user role (default)
- `admin` - Administrator role

**Security Features:**
- PKCE (Proof Key for Code Exchange) enabled
- Silent SSO check via iframe (`silent-check-sso.html`)
- Brute force protection enabled
- Password reset allowed
- Email verification supported

### Accessing Keycloak

**Via Reverse Proxy (Docker):**
- Admin Console: http://auth.localhost/admin
- Todo Realm: http://auth.localhost/realms/todo
- Account Console: http://auth.localhost/realms/todo/account
- OpenID Configuration: http://auth.localhost/realms/todo/.well-known/openid-configuration

**Credentials:**
- Admin: `admin` / `admin`

**Test User:**
- Username: `testuser`
- Email: `testuser@example.com`
- Password: `password123`
