using System.Net.Http;
using HR.Department.WebMvc.Common;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HR.Department.WebMvc.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class BaseController : Controller
    {
        public HttpClient HttpClient { get; }
        public ClaimManager ClaimManager { get; set; }


        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var clientFactory = httpContext.RequestServices.GetService<IHttpClientFactory>();

            HttpClient = clientFactory.CreateClient("MvcClient");
            ClaimManager = new ClaimManager(httpContext, httpContext.User);
            HttpClient.SetBearerToken(ClaimManager.AccessToken);
        }
    }
}
