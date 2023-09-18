using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using DataAccess.Dtos.ProductDto;
using DataAccess.Dtos.CategoryDto;
using System.Text;

namespace EStoreClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private string ProductApiUrl = "";
        private string CategoryApiUrl = "";
        public ProductsController()
        {
            _httpClient = new HttpClient();
            var contentType= new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "http://localhost:5121/api/products";
            CategoryApiUrl = "http://localhost:5121/api/categories";
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            HttpResponseMessage reponse= await _httpClient.GetAsync(ProductApiUrl);
            string datas= await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var products = JsonSerializer.Deserialize<List<ProductReadDto>>(datas,options);
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(ProductApiUrl+$"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var product = JsonSerializer.Deserialize<ProductReadDto>(data,options);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            HttpResponseMessage reponse = await _httpClient.GetAsync(CategoryApiUrl);
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cates = JsonSerializer.Deserialize<List<CategoryReadDto>>(datas, options);
            ViewData["CategoryId"] = new SelectList(cates, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductCreateDto product)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (ModelState.IsValid)
            {
                var requestData= JsonSerializer.Serialize(product);
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(requestData, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(ProductApiUrl)
                };
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(CategoryApiUrl);
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cates = JsonSerializer.Deserialize<List<CategoryReadDto>>(datas, options);
            ViewData["CategoryId"] = new SelectList(cates, "CategoryId", "CategoryName");
            return View(product);
        }

        //GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null )
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(ProductApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var product = JsonSerializer.Deserialize<ProductUpdateDto>(data, options);
            if (product == null)
            {
                return NotFound();
            }

            reponse = await _httpClient.GetAsync(CategoryApiUrl);
            string cateDatas = await reponse.Content.ReadAsStringAsync();
            var cates = JsonSerializer.Deserialize<List<CategoryReadDto>>(cateDatas, options);
            ViewData["CategoryId"] = new SelectList(cates, "CategoryId", "CategoryName");
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductUpdateDto product)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var requestData = JsonSerializer.Serialize(product);
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(requestData, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(ProductApiUrl)
                };
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            HttpResponseMessage reponse = await _httpClient.GetAsync(CategoryApiUrl);
            string datas = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cates = JsonSerializer.Deserialize<List<CategoryReadDto>>(datas, options);
            ViewData["CategoryId"] = new SelectList(cates, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage reponse = await _httpClient.GetAsync(ProductApiUrl + $"/{id.Value}");
            string data = await reponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var product = JsonSerializer.Deserialize<ProductReadDto>(data, options);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = HttpContext.Session;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {session.GetString("TOKEN")}");
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(ProductApiUrl+$"/{id}")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            return RedirectToAction(nameof(Index));
        }

    }
}
