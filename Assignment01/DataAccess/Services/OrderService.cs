using AutoMapper;
using BusinessObject.IRepo;
using BusinessObject.Models;
using DataAccess.Dtos.OrderDetailDto;
using DataAccess.Dtos.OrderDto;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Order> Create(OrderCreateDto createDto)
        {
            if(createDto == null) throw new ArgumentNullException(nameof(createDto));
            //Get max id
            var maxId = (await _unitOfWork.OrderRepository.GetAll()).Max(x => x.OrderId);
            //Create
            var order = _mapper.Map<Order>(createDto);
            order.OrderId = maxId + 1;
            var result= await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();
            await AddOrderDetail(createDto.OrderDetails!.ToList(), result.OrderId);
            return result;
        }

        private async Task AddOrderDetail(List<OrderDetailCreateDto> createDtos,int orderId) 
        {
            var orderDetails = _mapper.Map<List<OrderDetail>>(createDtos);
            for (int i = 0; i < createDtos.Count; i++)
            {
                orderDetails[i].OrderId = orderId;
            }
            await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var order= await _unitOfWork.OrderRepository.FindByField(x=>x.OrderId == id,x=>x.OrderDetails);
            if(order == null) throw new ArgumentNullException(nameof(order));
            _unitOfWork.OrderDetailRepository.SoftRemoveRange(order.OrderDetails.ToList());
            //await _unitOfWork.SaveChangeAsync();
            _unitOfWork.OrderRepository.SoftRemove(order);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<List<OrderReadDto>> GetAll()
        {
            var orders= await _unitOfWork.OrderRepository.GetAll(x=>x.Member!);
            return _mapper.Map<List<OrderReadDto>>(orders);
        }

        public async Task<List<OrderReadDto>> GetByMemberId(int memId)
        {
            var orders = await _unitOfWork.OrderRepository.FindListByField(x => x.MemberId == memId,x=>x.Member!);
            return _mapper.Map<List<OrderReadDto>>(orders);
        }

        public async Task<List<OrderDetailReadDto>> GetOrderDetailByOrderId(int orderId)
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.FindListByField(x => x.OrderId == orderId,x=>x.Product);
            return _mapper.Map<List<OrderDetailReadDto>>(orderDetails);
        }

        public async Task<OrderReadDto> GetById(int orderId)
        {

            var orders = await _unitOfWork.OrderRepository.FindByField(x => x.OrderId == orderId,x=>x.Member!);
            var result= _mapper.Map<OrderReadDto>(orders);
            var orderDetails = await _unitOfWork.OrderDetailRepository.FindListByField(x => x.OrderId == orderId, x => x.Product);
            result.OrderDetails= _mapper.Map<List<OrderDetailReadDto>>(orderDetails);
            return result;
        }
    }
}
