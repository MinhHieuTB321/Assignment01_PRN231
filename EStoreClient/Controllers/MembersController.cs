using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using DataAccess.Dtos.ProductDto;
using System.Text.Json;
using DataAccess.Dtos.MemberDto;
using System.Text;

namespace EStoreClient.Controllers
{
    public class MembersController : Controller
    {
        private readonly HttpClient _httpClient;
        private string MemberApiUrl = "";
        public MembersController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5121/api/members";
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl);
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var members = JsonSerializer.Deserialize<List<MemberReadDto>>(datas, options);
            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var member = JsonSerializer.Deserialize<MemberReadDto>(data, options);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,CompanyName,City,Country,Password")] MemberCreateDto member)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (ModelState.IsValid)
            {
                var requestData = JsonSerializer.Serialize(member);
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(requestData, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(MemberApiUrl)
                };
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null )
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var member = JsonSerializer.Deserialize<MemberReadDto>(data, options);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Email,CompanyName,City,Country,Password")] MemberReadDto member)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var requestData = JsonSerializer.Serialize(member);
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(requestData, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(MemberApiUrl)
                };
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var member = JsonSerializer.Deserialize<MemberReadDto>(data, options);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(MemberApiUrl + $"/{id}")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            return RedirectToAction(nameof(Index));
        }

    }
}
