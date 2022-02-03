using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PositionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PositionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("MvcClient");
        }

        [HttpGet]
        public async Task<IActionResult> GetTable()
        {
            var response = await _httpClient.GetAsync("position");

            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadFromJsonAsync(typeof(PositionListVm));
                return View("Table", (PositionListVm)contentStream);
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

        [HttpGet]
        public IActionResult Update(Guid id, string name, string description)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.Description = description;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(PositionForUpdateVm forUpdateVm)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(forUpdateVm),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponseMessage =
                await _httpClient.PutAsync("position", jsonContent);

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

        public async Task<IActionResult> DeleteEmployee(Guid positionId, Guid employeeId)
        {
            using var httpResponseMessage =
                await _httpClient.DeleteAsync($"position/{positionId}/{employeeId}");

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable", "Employee",
                new { positionId = positionId });
        }

        public IActionResult AddNewEmployee(Guid id)
        {
            ViewBag.PositionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(
            EmployeeForAddingToPositionVm employeeForAddingToPositionVm)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(employeeForAddingToPositionVm),
                Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponseMessage =
                await _httpClient.PostAsync("position/new/employee", jsonContent);

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable", "Employee",
                new { positionId = employeeForAddingToPositionVm.PositionId });
        }

        public async Task<IActionResult> AddExistingEmployee(Guid positionId)
        {
            var httpResponseMessage = await _httpClient.GetAsync("employee");
            var listEmployee = (await httpResponseMessage
                .Content.ReadFromJsonAsync(typeof(EmployeeListVm))) as EmployeeListVm;

            ViewBag.Employees =
                new SelectList(listEmployee?.EmployeeList, "Id", "Surname");

            ViewBag.PositionId = positionId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExistingEmployee(Guid positionId, Guid employeeId)
        {
            var httpResponseMessage =
                await _httpClient.PutAsync($"position/{positionId}/{employeeId}", default);

            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("GetTable", "Employee",
                new { positionId = positionId });
        }
    }
}
