using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using DataAccess.Dtos.MemberDto;
using System.Text.Json;
using DataAccess.Dtos.OrderDto;

namespace EStoreClient.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;
        private string OrderApiUrl = "";
        private string MemberApiUrl = "";

        public OrdersController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "http://localhost:5121/api/orders";
            MemberApiUrl = "http://localhost:5121/api/members";

        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            HttpResponseMessage reponse = await _httpClient.GetAsync(OrderApiUrl);
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var orders = JsonSerializer.Deserialize<List<OrderReadDto>>(datas, options);
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Orders/Create
        public async Task<IActionResult> Create()
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
            ViewData["MemberId"] = new SelectList(members, "MemberId", "Email");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(order);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
            //return View(order);
            return View();
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null || _context.Orders == null)
            //{
            //    return NotFound();
            //}

            //var order = await _context.Orders.FindAsync(id);
            //if (order == null)
            //{
            //    return NotFound();
            //}
            //ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
            //return View(order);
            return View();
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] Order order)
        {
            //if (id != order.OrderId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(order);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!OrderExists(order.OrderId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "City", order.MemberId);
            //return View(order);
            return View();
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(OrderApiUrl + $"/{id}")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            return RedirectToAction(nameof(Index));
        }
    }
}
