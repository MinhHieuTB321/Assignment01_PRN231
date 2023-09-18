using BusinessObject.Enums;
using DataAccess.Dtos.AuthenticationDto;
using DataAccess.IServices;
using DataAccess.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStoreAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IConfiguration _config;
        public AuthenticationController(IConfiguration configuration,IMemberService memberService)
        {
            _config = configuration;
            _memberService = memberService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginReadDto loginDto)
        {
            var adminAccount = new LoginReadDto
            {
                Email = _config["AdminAccount:Email"],
                Password = _config["AdminAccount:Pass"]
            };
            if(adminAccount.Email==loginDto.Email && adminAccount.Password==loginDto.Password) 
            {
                var token = Role.Admin.ToString().GenerateJsonWebToken(_config["SecretKey"]!);
                return Ok(new LoginResponseDto {MemberId=0, AccessToken=token,Role= Role.Admin.ToString() });
            }
            else
            {
                var token= await _memberService.CheckLogin(loginDto);
                return Ok(token);
            }
        }
    }
}
