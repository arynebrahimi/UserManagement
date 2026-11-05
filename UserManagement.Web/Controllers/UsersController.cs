using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Web.ViewModels.Account;

namespace UserManagement.Web.Controllers;

[Authorize(Policy = "AdminOnly")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(IUserService userService, IPasswordHasher<User> passwordHasher)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var username = model.Username.Trim();
        if (await _userService.UsernameExistsAsync(username))
        {
            ModelState.AddModelError(nameof(model.Username), "This username is already taken.");
            return View(model);
        }

        var user = new User
        {
            UserName = username,
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            CreatedAt = DateTime.UtcNow,
            Role = "User"
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        try
        {
            await _userService.AddAsync(user);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Unable to save the user. Please check the information and try again.");
            return View(model);
        }

        TempData["SuccessMessage"] = "User has been created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return View(new EditUserViewModel
        {
            ID = user.ID,
            Username = user.UserName,
            FullName = user.FullName,
            Email = user.Email
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditUserViewModel model)
    {
        if (id != model.ID)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var username = model.Username.Trim();
        if (await _userService.UsernameExistsAsync(username, id))
        {
            ModelState.AddModelError(nameof(model.Username), "This username is already used by another user.");
            return View(model);
        }

        user.UserName = username;
        user.FullName = model.FullName.Trim();
        user.Email = model.Email.Trim();

        await _userService.UpdateAsync(user);
        TempData["SuccessMessage"] = "User information has been updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        if (IsCurrentUser(id))
        {
            TempData["ErrorMessage"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Index));
        }

        return View(new DeleteUserViewModel
        {
            ID = user.ID,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        if (IsCurrentUser(id))
        {
            TempData["ErrorMessage"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Index));
        }

        await _userService.DeleteAsync(user);
        TempData["SuccessMessage"] = "User has been deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private bool IsCurrentUser(int id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(currentUserId, out var parsedId) && parsedId == id;
    }
}
