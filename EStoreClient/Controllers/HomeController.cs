using DataAccess.Dtos.AuthenticationDto;
using DataAccess.Dtos.MemberDto;
using EStoreClient.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EStoreClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private string AuthenApiUrl = "";

        public HomeController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            AuthenApiUrl = "http://localhost:5121/api/authentications";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Email,Password")] LoginReadDto loginDto)
        {
            var requestData = JsonSerializer.Serialize(loginDto);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(requestData, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(AuthenApiUrl)
            };
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                string datas=await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(datas, options);
                HttpContext.Session.SetString("TOKEN", loginResponse!.AccessToken!);
                if(loginResponse.MemberId!=0) HttpContext.Session.SetInt32("MemberId", loginResponse!.MemberId);
                if (loginResponse!.Role!.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Members");
                }
                else
                {
                    return RedirectToAction("Index", "Profile");
                }
            }
            return View(loginDto);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}