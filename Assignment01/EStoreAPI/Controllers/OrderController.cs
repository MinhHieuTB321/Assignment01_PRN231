using BusinessObject.Enums;
using DataAccess.Dtos.MemberDto;
using DataAccess.Dtos.OrderDto;
using DataAccess.IServices;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    public class OrderController:BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAll();
            return Ok(orders);
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("{id}/orders-detail")]
        public async Task<IActionResult> GetOrderDetailByOrderId(int id)
        {
            var orderDetails = await _orderService.GetOrderDetailByOrderId(id);
            return Ok(orderDetails);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var orderDetails = await _orderService.GetById(id);
            return Ok(orderDetails);
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto createDto)
        {
            var member = await _orderService.Create(createDto);
            return StatusCode(StatusCodes.Status201Created,"Create Successfully!");
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id);
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }
    }
}
