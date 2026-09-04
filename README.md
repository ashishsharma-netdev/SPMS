# SPMS - Student Registration

A .NET 8 ASP.NET Core MVC student registration module backed by Entity Framework Core and SQL Server.

## Features

- Student registration with server-side and client-side validation
- Unique email validation
- Student listing and search
- View student details
- Edit student information
- Delete student records
- Active/inactive status
- SQL Server support with EF Core migrations
- InMemory database fallback for local development
- Existing SPMS API functionality retained

## Tech Stack

- .NET 8
- C#
- ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server
- Bootstrap

## Run locally

1. Install .NET 8 SDK.
2. Open `SPMS.sln` in Visual Studio 2022.
3. Configure `ConnectionStrings:DefaultConnection` in `SPMS/appsettings.json` when using SQL Server.
4. Run the project.
5. The default page opens at `/Students`.

When no SQL Server connection string is configured, the app uses an InMemory database so the UI can be tested immediately.

## Main URLs

- `/Students` - student list/search
- `/Students/Create` - registration form
- `/Students/Details/{id}` - student details
- `/Students/Edit/{id}` - edit student

## Database

The `Student` entity is included in `AppDbContext` and has a unique index on `Email`. When SQL Server is configured, the application applies pending EF Core migrations on startup.
