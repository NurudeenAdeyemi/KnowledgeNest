using KnowledgeNest.Constants;
using KnowledgeNest.Models;
using KnowledgeNest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace KnowledgeNest.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IIdentityService _identityService;

        public AccountsController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Register()
        {
            var selectedRoles = _identityService.GetRoles();
            var roles = selectedRoles.Where(x => x.Name != RoleConstants.Admin);
            var model = new RegisterViewModel
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var registerModel = new RegisterModel
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Role = model.SelectedRole
            };

            var response = _identityService.Register(registerModel);
            if (response.Status)
            {
                ViewBag.Message = response.Message;
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = response.Message;
                return View();
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate the user's credentials 
                var response = _identityService.Login(model);
                
                if (response.Status)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, response.UserName),
                        new Claim(ClaimTypes.Email, response.Email)
                    };
                    foreach(var role in response.Roles)
                    {
                        new Claim(ClaimTypes.Role, role);
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
