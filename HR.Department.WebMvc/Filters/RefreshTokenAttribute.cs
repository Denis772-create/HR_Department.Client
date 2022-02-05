using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using HR.Department.WebMvc.Common;
using HR.Department.WebMvc.Controllers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.Department.WebMvc.Filters
{
    public class RefreshTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = (BaseController)context.Controller;
            var claimManager = controller.ClaimManager;
            var httpClient = controller.HttpClient;
            var httpContext = controller.HttpContext;
            string currentToken = controller.ClaimManager.AccessToken;

            if (currentToken != null)
            {
                var jwtToken = new JwtSecurityTokenHandler()
                    .ReadToken(currentToken);
                var expDate = jwtToken.ValidTo;

                if (expDate < DateTime.UtcNow)
                {
                    var resultRefreshToken = await httpClient.RequestRefreshTokenAsync(
                        new RefreshTokenRequest
                        {
                            Address = "https://localhost:6001/connect/token",
                            ClientId = "hr_department_mvc_id",
                            ClientSecret = "hr_department_mvc_secret",
                            RefreshToken = claimManager.RefreshToken,
                            Scope = "openid DepartmentAPI"
                        });

                    var authenticate = await httpContext.AuthenticateAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    authenticate.Properties.UpdateTokenValue("access_token", resultRefreshToken.AccessToken);
                    authenticate.Properties.UpdateTokenValue("refresh_token", resultRefreshToken.RefreshToken);

                    await httpContext.SignInAsync(authenticate.Principal, authenticate.Properties);
                    claimManager = new ClaimManager(httpContext, httpContext.User);
                    httpClient.SetBearerToken(claimManager.AccessToken);
                }
            }

            await next();
        }

    }
}
