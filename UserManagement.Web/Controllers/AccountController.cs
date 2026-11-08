
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using UserManagement.Web.ViewModels.Account;

namespace UserManagement.Web.Controllers;

public class AccountController : Controller
{
    private readonly IRegisterUser _registerUser;
    private readonly IAuthenticateUser _authenticateUser;

    public AccountController(
        IRegisterUser registerUser,
        IAuthenticateUser authenticateUser)
    {
        _registerUser = registerUser;
        _authenticateUser = authenticateUser;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _registerUser.RegisterAsync(
            model.Username,
            model.FullName,
            model.Email,
            model.Password);

        if (!result)
        {
            ModelState.AddModelError(
                nameof(model.Username),
                "This username is already taken.");

            return View(model);
        }

        TempData["SuccessMessage"] =
            "Registration completed. You can now log in.";

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var user = await _authenticateUser.AuthenticateAsync(
            model.Username,
            model.Password);

        if (user == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Username or password is incorrect.");

            return View(model);
        }

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.ID.ToString()),

            new(
                ClaimTypes.Name,
                user.UserName),

            new(
                ClaimTypes.Role,
                user.Role)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);

        if (!string.IsNullOrEmpty(returnUrl) &&
            Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return Content(
            "You do not have permission to access this page.");
    }
}
