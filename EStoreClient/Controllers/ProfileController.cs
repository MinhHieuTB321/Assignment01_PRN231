using BusinessObject.Models;
using DataAccess.Dtos.MemberDto;
using DataAccess.Dtos.OrderDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EStoreClient.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;
        private string MemberApiUrl = "";
        private string OrderApiUrl = "";

        public ProfileController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "http://localhost:5121/api/members";
            OrderApiUrl = "http://localhost:5121/api/orders";

        }

        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            var id = session.GetInt32("MemberId");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl + $"/{id}");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("MemberId,Email,CompanyName,City,Country,Password")] MemberReadDto member)
        
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
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

        // GET: Profile/Order
        public async Task<IActionResult> Order()
        {
            var session = HttpContext.Session;
            var id = session.GetInt32("MemberId");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            HttpResponseMessage reponse = await _httpClient.GetAsync(MemberApiUrl+$"/{id}/orders");
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orders = JsonSerializer.Deserialize<List<OrderReadDto>>(datas, options);
            return View(orders);
        }

        // GET: OrderDetail/5
        public async Task<IActionResult> OrderDetail(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(OrderApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var order = JsonSerializer.Deserialize<OrderReadDto>(data, options);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
