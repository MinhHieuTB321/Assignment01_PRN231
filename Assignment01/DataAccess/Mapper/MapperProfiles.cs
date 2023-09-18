using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dtos.CategoryDto;
using DataAccess.Dtos.MemberDto;
using DataAccess.Dtos.OrderDetailDto;
using DataAccess.Dtos.OrderDto;
using DataAccess.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class MapperProfiles:Profile
    {
        public MapperProfiles()
        {
            #region Product
            CreateMap<ProductReadDto, Product>()
                .ForPath(x=>x.Category!.CategoryName,opt=>opt.MapFrom(x=>x.CategoryName))
                .ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            #endregion

            #region Member
            CreateMap<Member, MemberReadDto>().ReverseMap();
            CreateMap<MemberCreateDto, Member>();
            #endregion

            #region Order
            CreateMap<OrderReadDto,Order>()
                .ForPath(x => x.Member!.Email, opt => opt.MapFrom(x => x.Member))
                .ForMember(x=>x.OrderDetails,opt=>opt.Ignore())
                .ReverseMap();
            CreateMap<OrderCreateDto, Order>()
                .ForMember(x=>x.OrderDetails,opt=>opt.Ignore());
            #endregion

            #region OrderDetail
            CreateMap<OrderDetailReadDto, OrderDetail>()
                .ForPath(x => x.Product!.ProductName, opt => opt.MapFrom(x => x.ProductName))
                .ReverseMap();
            CreateMap<OrderDetailCreateDto, OrderDetail>();
            #endregion

            #region Category
            CreateMap<Category, CategoryReadDto>();
            #endregion

        }
    }
}
