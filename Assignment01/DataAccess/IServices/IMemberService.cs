using BusinessObject.Models;
using DataAccess.Dtos.AuthenticationDto;
using DataAccess.Dtos.MemberDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IMemberService
    {
        Task<List<MemberReadDto>> GetAll();
        Task<MemberReadDto> GetById(int id);
        Task<LoginResponseDto> CheckLogin(LoginReadDto loginReadDto);
        Task<MemberReadDto> Create(MemberCreateDto memberCreateDto);
        Task<bool> Update(MemberReadDto memberReadDto);
        Task<bool> Delete(int id);
    }
}
