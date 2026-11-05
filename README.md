# UserManagement - Simplified Final

ASP.NET Core MVC user-management application using a simplified layered structure:
- Domain: User entity
- Application: IDb abstraction and IUserService / UserService
- Web (Presentation): MVC controllers, views, EF Core AppDbContext, migrations and static assets

## UI
The UI includes:
- Sign-in and registration pages
- Authenticated application shell with sidebar and top bar
- Dashboard landing page
- User directory with summary cards and actions
- Add, edit and delete user pages
- Responsive CSS in `wwwroot/css/custom.css`

## Run
1. Open `UserManagement.slnx` in Visual Studio 2026.
2. Set `UserManagement.Web` as the startup project.
3. Restore NuGet packages and build the solution.
4. Check the SQL Server LocalDB connection string in `UserManagement.Web/appsettings.json`.
5. Apply migrations using Package Manager Console:
   `Update-Database -Project UserManagement.Web -StartupProject UserManagement.Web -Context AppDbContext`
6. Run the application. Unauthenticated visitors are redirected to `/Account/Login`.
7. Register an account. To access the Users administration page, assign the `Admin` role to the intended account in the database.

## Notes
- Passwords are hashed with ASP.NET Core `PasswordHasher<User>`.
- Admin-only user management is protected by the `AdminOnly` authorization policy.
- The application uses EF Core directly through the `IDb` abstraction; there is no Infrastructure project or Repository implementation.
