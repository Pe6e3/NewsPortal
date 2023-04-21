using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using News.Models;
using News.ViewModels;
using System.Security.Claims;
using News.Data;
using Microsoft.EntityFrameworkCore;

namespace News.Controllers;

public class AccountController : Controller
{
    private NewsContext _db;
    public AccountController(NewsContext db)
    {
        _db = db;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == model.UserName &&
                                             u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user); // аутентификация


                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user == null)
            {
                Role role = _db.Roles.FirstOrDefault(r => r.RoleName == "user");
                user = new User { UserName = model.UserName, Password = model.Password, RoleId = 2, Role = role };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }





    private async Task Authenticate(User user)
    {
        var claims = new List<Claim>
       {
           new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
               new Claim(ClaimsIdentity.DefaultNameClaimType, user.Role?.RoleName)
       };
        ClaimsIdentity id = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(id),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
            }
 );
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
