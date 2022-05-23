using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.Department.WebMvc.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult AuthIndex()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            var parameters = new AuthenticationProperties
            {
                RedirectUri = "Home/Index"
            };

            return SignOut(parameters,
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
