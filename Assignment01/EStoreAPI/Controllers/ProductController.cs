using BusinessObject.Enums;
using DataAccess.Dtos.MemberDto;
using DataAccess.Dtos.ProductDto;
using DataAccess.IServices;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    public class ProductController:BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProduct();
            return Ok(products);
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto createDto)
        {
            var product = await _productService.Create(createDto);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto updateDto)
        {
            var result = await _productService.Update(updateDto);
            if (!result)
            {
                return BadRequest("Update Fail!");
            }
            return Ok("Update Successfully!");
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }
    }
}
