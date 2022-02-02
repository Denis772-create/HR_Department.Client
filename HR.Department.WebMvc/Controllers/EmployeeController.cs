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


        [HttpGet]
        public async Task<IActionResult> GetTable(Guid id)
        {
            var response = await _httpClient.GetAsync($"employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadFromJsonAsync(typeof(EmployeeListVm));
                return View("Table", (EmployeeListVm)contentStream);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var typesHttpresponse = await _httpClient.GetAsync("position/types");
            if (typesHttpresponse.IsSuccessStatusCode)
            {
                var listOfTypes = (await typesHttpresponse.Content
                    .ReadFromJsonAsync(typeof(IEnumerable<PositionTypeVm>))) as IEnumerable<PositionTypeVm>;

                ViewBag.PositionTypes = new SelectList(listOfTypes, "Id", "Name");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositionForCreateVm positionForCreateVm)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(positionForCreateVm),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage =
                await _httpClient.PostAsync("position", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable");
        }

        //[HttpGet]
        //public IActionResult EditAddress(Guid id, string city, string country, string street)
        //{
        //    //ViewBag.Id = id;
        //    //ViewBag.Name = name;
        //    //ViewBag.Description = description;
        //    //return View();
        //}

        [HttpPost]
        public async Task<IActionResult> EditAddress(Guid id, string country, string city, string street)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(new { country, city, street }),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage =
                await _httpClient.PutAsync($"employee/{id}", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            using var httpResponseMessage =
                await _httpClient.DeleteAsync($"position/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable");
        }

    }
}
