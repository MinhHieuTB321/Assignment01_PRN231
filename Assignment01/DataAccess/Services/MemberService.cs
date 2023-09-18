using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.IRepo;
using BusinessObject.Models;
using DataAccess.Dtos.AuthenticationDto;
using DataAccess.Dtos.MemberDto;
using DataAccess.IServices;
using DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IConfiguration configuration)
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
            _config= configuration;
        }

        public async Task<LoginResponseDto> CheckLogin(LoginReadDto login)
        {
            var member= await _unitOfWork.MemberRepository.FindByField(x=>x.Email==login.Email&&x.Password==login.Password);
            if(member==null)throw new Exception("Incorrect Email or Password!");
            var token = Role.Customer.ToString().GenerateJsonWebToken(_config["SecretKey"]!);
            return new LoginResponseDto
            {
                MemberId=member.MemberId,
                AccessToken = token,
                Role = Role.Customer.ToString()
            };
        }

        public async Task<MemberReadDto> Create(MemberCreateDto memberCreateDto)
        {
           if(memberCreateDto == null) throw new Exception();
           //Get max id
           var maxId = (await _unitOfWork.MemberRepository.GetAll()).Max(x => x.MemberId);
            //Create member
           var member = _mapper.Map<Member>(memberCreateDto);
            member.MemberId = maxId+1;
           var result= await _unitOfWork.MemberRepository.AddAsync(member);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<MemberReadDto>(result);
        }

        public async Task<bool> Delete(int id)
        {
            var member= await _unitOfWork.MemberRepository.FindByField(x=>x.MemberId==id);
            if(member==null) throw new ArgumentNullException(nameof(member));
            _unitOfWork.MemberRepository.SoftRemove(member);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<List<MemberReadDto>> GetAll()
        {
            var members = await _unitOfWork.MemberRepository.GetAll();
            return _mapper.Map<List<MemberReadDto>>(members);
        }

        public async Task<MemberReadDto> GetById(int id)
        {
            var member = await _unitOfWork.MemberRepository.FindByField(x=>x.MemberId == id);
            return _mapper.Map<MemberReadDto>(member);
        }

        public async Task<bool> Update(MemberReadDto memberReadDto)
        {
            var member= await _unitOfWork.MemberRepository.FindByField(x=>x.MemberId==memberReadDto.MemberId);
            member = _mapper.Map(memberReadDto, member);
            _unitOfWork.MemberRepository.Update(member);
            return await _unitOfWork.SaveChangeAsync();
        }
    }
}
