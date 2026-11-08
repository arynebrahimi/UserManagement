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
    private readonly IAddUser _addUser;
    private readonly IDeleteUser _deleteUser;
    private readonly IUpdateUser _updateUser;
    private readonly IGetAllUsers _getAllUsers;
    private readonly IGetUserById _getUserById;
    private readonly IUsernameExists _usernameExists;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(
        IAddUser addUser,
        IDeleteUser deleteUser,
        IUpdateUser updateUser,
        IGetAllUsers getAllUsers,
        IGetUserById getUserById,
        IUsernameExists usernameExists,
        IPasswordHasher<User> passwordHasher)
    {
        _addUser = addUser;
        _deleteUser = deleteUser;
        _updateUser = updateUser;
        _getAllUsers = getAllUsers;
        _getUserById = getUserById;
        _usernameExists = usernameExists;
        _passwordHasher = passwordHasher;
    }

    // GET: Users
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _getAllUsers.GetAllAsync();

        return View(users);
    }

    // GET: Users/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var username = model.Username.Trim();

        if (await _usernameExists.ExecuteAsync(username))
        {
            ModelState.AddModelError(
                nameof(model.Username),
                "This username is already taken.");

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

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            model.Password);

        try
        {
            await _addUser.AddAsync(user);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "Unable to save the user. Please try again.");

            return View(model);
        }

        TempData["SuccessMessage"] =
            "User has been created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: Users/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _getUserById.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        var model = new EditUserViewModel
        {
            ID = user.ID,
            Username = user.UserName,
            FullName = user.FullName,
            Email = user.Email
        };

        return View(model);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        EditUserViewModel model)
    {
        if (id != model.ID)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var user = await _getUserById.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        var username = model.Username.Trim();

        if (await _usernameExists.ExecuteAsync(username, id))
        {
            ModelState.AddModelError(
                nameof(model.Username),
                "This username is already used by another user.");

            return View(model);
        }

        user.UserName = username;
        user.FullName = model.FullName.Trim();
        user.Email = model.Email.Trim();

        await _updateUser.UpdateAsync(user);

        TempData["SuccessMessage"] =
            "User information has been updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: Users/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _getUserById.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        if (IsCurrentUser(id))
        {
            TempData["ErrorMessage"] =
                "You cannot delete your own account.";

            return RedirectToAction(nameof(Index));
        }

        var model = new DeleteUserViewModel
        {
            ID = user.ID,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email
        };

        return View(model);
    }

    // POST: Users/DeleteConfirmed/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _getUserById.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        if (IsCurrentUser(id))
        {
            TempData["ErrorMessage"] =
                "You cannot delete your own account.";

            return RedirectToAction(nameof(Index));
        }

        await _deleteUser.DeleteAsync(user);

        TempData["SuccessMessage"] =
            "User has been deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    private bool IsCurrentUser(int id)
    {
        var currentUserId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return int.TryParse(currentUserId, out var parsedId)
               && parsedId == id;
    }
}