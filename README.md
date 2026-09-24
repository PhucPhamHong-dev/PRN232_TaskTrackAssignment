# PRN232 TaskTrack Assignment

TaskTrack is a public task and team management application for PRN232 Assignment 1.

## Repository structure

- `backend/` — ASP.NET Core .NET 8 Web API split into API, Service, and Repository projects.
- `frontend/` — Next.js App Router application using TypeScript and Tailwind CSS.

## Configuration

Copy `.env.example` to a private environment file and set the PostgreSQL URL and frontend API URL. Never commit real passwords or tokens.

For local backend development, copy `backend/TaskTrack.API/appsettings.Local.example.json` to `appsettings.Local.json` and replace the placeholder connection string. The local file is ignored by Git.

The backend accepts a PostgreSQL URL through `DATABASE_URL` and converts it for Npgsql. The SQL supplied for the assignment should be run once against a new PostgreSQL database before scaffolding or testing the API.

## Local commands

Backend:

```bash
cd backend
dotnet build StudentID_ClassCode_Ass1_BE.sln
dotnet run --project TaskTrack.API
```

Frontend:

```bash
cd frontend/StudentID_ClassCode_Ass1_FE
npm install
npm run dev
```

Swagger is available at the backend URL followed by `/swagger`.
