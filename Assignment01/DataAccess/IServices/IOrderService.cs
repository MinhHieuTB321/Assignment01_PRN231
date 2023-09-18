using BusinessObject.Models;
using DataAccess.Dtos.MemberDto;
using DataAccess.Dtos.OrderDetailDto;
using DataAccess.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IOrderService
    {
        Task<List<OrderReadDto>> GetAll();
        Task<List<OrderReadDto>> GetByMemberId(int memId);
        Task<OrderReadDto> GetById(int orderId);
        Task<Order> Create(OrderCreateDto createDto);
        Task<bool> Delete(int id);
        Task<List<OrderDetailReadDto>> GetOrderDetailByOrderId(int orderId);
    }
}
