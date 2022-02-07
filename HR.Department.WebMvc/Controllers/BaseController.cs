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
        private readonly IHttpClientFactory _clientFactory;
        public HttpClient HttpClient { get; }
        public ClaimManager ClaimManager { get; set; }


        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            _clientFactory = httpContext.RequestServices.GetService<IHttpClientFactory>();

            HttpClient = _clientFactory.CreateClient("MvcClient");
            ClaimManager = new ClaimManager(httpContext, httpContext.User);
            HttpClient.SetBearerToken(ClaimManager.AccessToken);
        }
    }
}
