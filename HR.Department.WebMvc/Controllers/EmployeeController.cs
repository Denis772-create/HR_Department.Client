using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HR.Department.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR.Department.WebMvc.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("MvcClient");
        }

        [HttpGet]
        public async Task<IActionResult> TableEmployeeByPositions()
        {
            var response = await _httpClient.GetAsync("position");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadFromJsonAsync(typeof(PositionListVm));
                return View("TableEmployeeByPositions", (PositionListVm)contentStream);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetTable(string positionId)
        {
            ViewBag.PositionId = positionId;
            var response = await _httpClient.GetAsync($"employee/{ViewBag.PositionId}");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadFromJsonAsync(typeof(EmployeeListVm));
                return View("Table", (EmployeeListVm)contentStream);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(Guid id, Guid positionId, string country, string city, string street)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(new { country, city, street }),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage =
                await _httpClient.PutAsync($"employee/{id}", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable", new { positionId = positionId });
        }


    }
}
