# Architecture

This solution follows the simplified arrangement requested by the mentor.

## Projects

- `UserManagement.Domain`: domain entities (`User`). It has no project dependencies.
- `UserManagement.Application`: application contract (`IUserService`), implementation (`UserService`), and database abstraction (`IDb`). It references Domain.
- `UserManagement.Web` (Presentation): MVC controllers and views, authentication, EF Core `AppDbContext`, and migrations. `AppDbContext` implements `IDb`.

There is no Infrastructure project and no Repository / UnitOfWork implementation.

## Request flow

`Browser -> MVC Controller -> IUserService -> UserService -> IDb -> AppDbContext -> SQL Server`

Controllers do not access `DbContext` directly. `UserService` receives `IDb` by constructor injection. `Program.cs` registers `AppDbContext`, maps `IDb` to the same scoped context instance, and registers `IUserService` to `UserService`.

## Design note

The mentor's requested `IDb` abstraction exposes EF Core's `DbSet<User>`, so Application references the EF Core abstractions package. This is a deliberate simplified design matching the requested arrangement. A stricter persistence-agnostic Clean Architecture would instead keep EF Core types out of Application and place the concrete DbContext in a separate persistence/Infrastructure project.
