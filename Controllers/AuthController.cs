using gaas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gaas.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _users;
    private readonly SignInManager<IdentityUser> _signIn;

    public AuthController(
        UserManager<IdentityUser> users,
        SignInManager<IdentityUser> signIn)
    {
        _users = users;
        _signIn = signIn;
    }

    [HttpGet, AllowAnonymous]
    public IActionResult SignUp() => View(new SignUpViewModel());

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _users.CreateAsync(user, model.Password);

        if (result.Succeeded)
            return RedirectToAction(nameof(Login));

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpGet, AllowAnonymous]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signIn.PasswordSignInAsync(
            userName: model.Email,
            password: model.Password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Unable to sign in with those details.");
        return View(model);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet, AllowAnonymous]
    public IActionResult AccessDenied() => StatusCode(403);
}