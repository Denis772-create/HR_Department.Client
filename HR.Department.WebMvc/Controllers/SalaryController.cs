using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HR.Department.WebMvc.Controllers
{
    public class SalaryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SalaryController(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        public async Task<IActionResult> UpdateSalary()
        {
            var client = _httpClientFactory.CreateClient("MvcClient");
            var response = await client
                .PutAsync("employee", default);

            if (response.IsSuccessStatusCode)
            {
                var count = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index", new { count });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("{count}")]
        public IActionResult Index(string count)
        {
            return View("Index", count);
        }
    }
}
