using InvoiceGenerator2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator2.Core.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);
        if (result.IsLockedOut)
            return View("Lockout");

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    //
    // GET: /Account/ResetPassword
    [Authorize(Roles = $"{RoleName.Admin}, {RoleName.Invoicing}")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    //
    // POST: /Account/ResetPassword
    [HttpPost]
    [Authorize(Roles = $"{RoleName.Admin}, {RoleName.Invoicing}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var id = RouteData.Values["id"]?.ToString();
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return RedirectToAction(nameof(ResetPasswordConfirmation));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (result.Succeeded)
            return RedirectToAction(nameof(ResetPasswordConfirmation));

        AddErrors(result);
        return View(model);
    }

    //
    // GET: /Account/ResetPasswordConfirmation
    [Authorize(Roles = $"{RoleName.Admin}, {RoleName.Invoicing}")]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    //
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleName.Admin}, {RoleName.Invoicing}")]
    public async Task<IActionResult> LogOff()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    #endregion
}
