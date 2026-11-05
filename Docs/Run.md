# Run guide

1. Open `UserManagement.slnx` in Visual Studio 2026.
2. Select `UserManagement.Web` as Startup Project.
3. Restore NuGet packages and run Rebuild Solution.
4. Verify `DefaultConnection` in `UserManagement.Web/appsettings.json`.
5. Run `Update-Database -Project UserManagement.Web -StartupProject UserManagement.Web -Context AppDbContext` in Package Manager Console.
6. Start the application. The root page redirects unauthenticated users to `/Account/Login`.
7. Register, then sign in. Admin-only functions require the `Admin` role.

Static files are served from `wwwroot`. Main styles are in `wwwroot/css/custom.css`.
