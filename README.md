# PRN232 TaskTrack Assignment 1

TaskTrack is a public task and team management application built with ASP.NET Core 8, PostgreSQL, Entity Framework Core Database-First, Next.js App Router, TypeScript, and Tailwind CSS.

## Live application

- Frontend: [https://prn-232-task-track-assignment.vercel.app](https://prn-232-task-track-assignment.vercel.app)
- Backend Swagger: [https://prn232-tasktrack-api.onrender.com/swagger](https://prn232-tasktrack-api.onrender.com/swagger)
- Source repository: [PhucPhamHong-dev/PRN232_TaskTrackAssignment](https://github.com/PhucPhamHong-dev/PRN232_TaskTrackAssignment)

## Features

- Public dashboard with live department, project, and task totals.
- Department, project, task, and tag CRUD without authentication.
- Department and project details with related data.
- Task search by title, status, priority, project, and tag.
- Status and priority badges, modal forms, delete confirmations, validation, loading states, error states, and toast notifications.
- Soft-delete for tasks and relationship checks before deleting departments, projects, or tags.
- Swagger API documentation and GitHub Actions build/lint checks.
- Responsive layouts for desktop and mobile.

## Architecture

```mermaid
flowchart LR
    UI[Next.js frontend] -->|REST / JSON| API[TaskTrack.API]
    API --> Service[TaskTrack.Service]
    Service --> Repo[TaskTrack.Repo]
    Repo --> EF[EF Core Database-First]
    EF --> DB[(PostgreSQL)]
```

The backend follows the required layered design. Controllers only call services; services contain validation and business rules; all database access is isolated in the repository project.

## Database ERD

```mermaid
erDiagram
    Department ||--o{ Project : has
    Project ||--o{ Task : has
    Task ||--o{ TaskTag : uses
    Tag ||--o{ TaskTag : assigned

    Department {
        int DepartmentID PK
        string DepartmentName
        string DepartmentDescription
        bool IsActive
    }
    Project {
        int ProjectID PK
        int DepartmentID FK
        string ProjectName
        date StartDate
        date EndDate
        smallint Status
        bool IsActive
    }
    Task {
        int TaskID PK
        int ProjectID FK
        string Title
        smallint Status
        smallint Priority
        date DueDate
        bool IsActive
    }
    Tag {
        int TagID PK
        string TagName
        string Color
    }
    TaskTag {
        int TaskID PK,FK
        int TagID PK,FK
    }
```

## Repository structure

- `backend/TaskTrack.API` — controllers, middleware, Swagger, CORS, and application startup.
- `backend/TaskTrack.Service` — DTOs, service interfaces, validation, and business logic.
- `backend/TaskTrack.Repo` — scaffolded EF Core entities, DbContext, and repository implementation.
- `frontend/StudentID_ClassCode_Ass1_FE` — Next.js public and management pages.
- `.github/workflows/ci.yml` — backend build and frontend lint/build verification.

## Local setup

Requirements: .NET 8 SDK, Node.js 22 or later, and a PostgreSQL database initialized with the supplied `TaskManagementDB_Postgres.sql` file.

1. Clone the repository.
2. Run the supplied SQL script against PostgreSQL without changing its schema.
3. Copy `backend/TaskTrack.API/appsettings.Local.example.json` to `backend/TaskTrack.API/appsettings.Local.json` and enter the PostgreSQL connection string. This private file is ignored by Git.
4. Start the backend:

   ```bash
   cd backend
   dotnet restore StudentID_ClassCode_Ass1_BE.sln
   dotnet run --project TaskTrack.API
   ```

5. Create `frontend/StudentID_ClassCode_Ass1_FE/.env.local`:

   ```env
   NEXT_PUBLIC_API_URL=http://localhost:5187
   ```

6. Start the frontend:

   ```bash
   cd frontend/StudentID_ClassCode_Ass1_FE
   npm install
   npm run dev
   ```

The frontend runs at `http://localhost:3000`. Swagger uses the backend URL followed by `/swagger`.

## Environment variables

| Application | Variable | Example / purpose |
| --- | --- | --- |
| Backend | `DATABASE_URL` | PostgreSQL URL supplied by the hosting provider |
| Backend | `ASPNETCORE_ENVIRONMENT` | `Production` on Render |
| Backend | `FRONTEND_URL` | Exact Vercel origin allowed by CORS |
| Frontend | `NEXT_PUBLIC_API_URL` | Public Render backend URL, without a trailing slash |

Never commit passwords or production connection strings. See `.env.example` for placeholders.

## Deploy backend to Render

1. Push the backend code to a public GitHub repository.
2. Create a Render **Web Service** from that repository.
3. Choose **Docker** as the runtime. If using this monorepo, set the root directory to `backend`.
4. Set `DATABASE_URL`, `ASPNETCORE_ENVIRONMENT=Production`, and `FRONTEND_URL` in Render.
5. Deploy and verify `https://YOUR-SERVICE.onrender.com/swagger`.

The included `backend/Dockerfile` listens on Render's assigned `PORT` and publishes the .NET 8 API.

## Deploy frontend to Vercel

1. Push the frontend code to its separate public GitHub repository, as required by the assignment.
2. Import the repository into Vercel and set the project root to `frontend/StudentID_ClassCode_Ass1_FE` only if deploying from this monorepo.
3. Add `NEXT_PUBLIC_API_URL=https://YOUR-SERVICE.onrender.com`.
4. Deploy, copy the final Vercel URL into the backend's `FRONTEND_URL`, and redeploy the backend if needed.
5. Verify every public and management route against real PostgreSQL data.

## Main routes

| Public pages | Management pages |
| --- | --- |
| `/` | `/departments/manage` |
| `/departments` | `/projects/manage` |
| `/departments/[id]` | `/tasks/manage` |
| `/projects/[id]` | `/tags/manage` |
| `/tasks/[id]` |  |
| `/search` |  |

The API exposes all 24 endpoints specified for departments, projects, tasks, and tags under `/api`.
