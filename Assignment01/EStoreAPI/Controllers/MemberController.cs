using BusinessObject.Enums;
using DataAccess.Dtos.MemberDto;
using DataAccess.IServices;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    public class MemberController:BaseController
    {
        private readonly IMemberService _memberService;
        private readonly IOrderService _orderService;

        public MemberController(IMemberService memberService, IOrderService orderService)
        {
            _memberService = memberService;
            _orderService = orderService;
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _memberService.GetAll();
            return Ok(members);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _memberService.GetById(id);
            return Ok(member);
        }
        [Authorize]
        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrderByMemberId(int id)
        {
            var orders = await _orderService.GetByMemberId(id);
            return Ok(orders);
        }
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost]
        public async Task<IActionResult> Create(MemberCreateDto createDto)
        {
            var member = await _memberService.Create(createDto);
            return CreatedAtAction(nameof(GetById),new {id=member.MemberId},member);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(MemberReadDto updateDto)
        {
            var result = await _memberService.Update(updateDto);
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
            var result = await _memberService.Delete(id);
            if (!result)
            {
                return BadRequest("Delete Fail!");
            }
            return Ok("Delete Successfully!");
        }

    }
}
