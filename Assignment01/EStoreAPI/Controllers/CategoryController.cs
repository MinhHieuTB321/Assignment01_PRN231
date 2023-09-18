using BusinessObject.Enums;
using DataAccess.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var cates = await _categoryService.GetAllCategory();
            return Ok(cates);
        }
    }
}
