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
- **Mediator 3.0.1** - CQRS pattern implementation
- **FluentValidation 12.1.1** - Request validation
- **Ardalis.Result 10.1.0** - Result pattern for API responses
- **Serilog 10.0.0** - Structured logging with enrichers

### Frontend (App)

- **React 19.2.0** - UI library
- **React Router 7.13.0** - Full-stack routing and framework
- **TypeScript 5.9.3** - Type-safe JavaScript
- **Vite 7.3.1** - Build tool and dev server
- **Tailwind CSS 4.1.18** - Utility-first CSS framework

#### Key Libraries

- **Axios 1.13.5** - HTTP client for API communication
- **React Hot Toast 2.6.0** - Toast notifications
- **Lodash 4.17.23** - Utility functions

### Database

- **SQL Server 2025** (latest) - Application database running in Docker container
- **PostgreSQL 17** - Keycloak database running in Docker container

### Authentication

- **Keycloak 26.0.5** - Open Source Identity and Access Management
  - OpenID Connect (OIDC) provider
  - Single Sign-On (SSO) capabilities
  - User management and authentication

### DevOps & Tooling

- **Docker** & **Docker Compose** - Containerization and orchestration
- **ESLint** - JavaScript/TypeScript linting
- **Prettier** - Code formatting
- **Target OS**: Linux (ARM64)

## Project Structure

```
secure-todo/
├── api/                              # Backend .NET application
│   ├── SecureTodo.Api/              # API layer (controllers, endpoints)
│   ├── SecureTodo.Application/      # Application layer (business logic, CQRS)
│   ├── SecureTodo.Domain/           # Domain layer (entities, interfaces)
│   ├── SecureTodo.Infrastructure/   # Infrastructure layer (EF Core, data access)
│   ├── Directory.Build.props        # Shared build configuration
│   └── Directory.Packages.props     # Centralized package management
├── app/                             # Frontend React application
│   ├── src/                         # Source files
│   ├── package.json                 # Node dependencies
│   ├── vite.config.ts              # Vite configuration
│   └── react-router.config.ts      # React Router configuration
├── keycloak/                        # Keycloak configuration
│   └── import/                      # Realm import files
│       └── todo-realm.json         # Todo realm with client and users
├── docker-compose.yml               # Docker services configuration
└── scripts/                         # Utility scripts
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js 20+ and npm
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

### Accessing Services

**Keycloak Admin Console:**
- URL: http://localhost:8180/admin
- Username: `admin`
- Password: `admin`

**Keycloak Todo Realm:**
- URL: http://localhost:8180/realms/todo
- Account Console: http://localhost:8180/realms/todo/account
- OpenID Configuration: http://localhost:8180/realms/todo/.well-known/openid-configuration

**Test User Login:**
- Username: `testuser`
- Email: `testuser@example.com`
- Password: `password123`

### Running the API

```bash
cd api/SecureTodo.Api
dotnet run
```

### Running the Frontend

```bash
cd app
npm install
npm run dev
```

## Development

### Frontend

- `npm run dev` - Start development server with hot reload
- `npm run build` - Build for production
- `npm run lint` - Run ESLint
- `npm run lint:format` - Format code with Prettier

### Backend

The API follows Clean Architecture principles:

- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Business logic, CQRS handlers, validation
- **Infrastructure Layer**: Data access, EF Core, external services
- **API Layer**: HTTP endpoints, middleware, configuration

## Architecture Patterns

- **Clean Architecture** - Dependency inversion and separation of concerns
- **CQRS** - Command Query Responsibility Segregation via Mediator
- **Result Pattern** - Explicit success/failure handling
- **Repository Pattern** - Data access abstraction

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

### Integration with React

To integrate the React app with Keycloak:

1. Install Keycloak JS adapter:
```bash
npm install keycloak-js
```

2. Configure Keycloak client:
```javascript
import Keycloak from 'keycloak-js';

const keycloak = new Keycloak({
  url: 'http://localhost:8180',
  realm: 'todo',
  clientId: 'todo-app'
});
```

3. Initialize and authenticate:
```javascript
keycloak.init({ 
  onLoad: 'login-required',
  pkceMethod: 'S256'
}).then(authenticated => {
  if (authenticated) {
    // User is authenticated, access token available
    console.log('Token:', keycloak.token);
  }
});
```
