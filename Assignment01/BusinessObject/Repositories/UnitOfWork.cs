using BusinessObject.IRepo;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FstoreDbContext _db;
        private readonly IMemberRepository _memberRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _cateRepo;

        public UnitOfWork(FstoreDbContext fstore,
            IMemberRepository memberRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _db = fstore;
            _memberRepo= memberRepository;
            _orderRepo = orderRepository;
            _orderDetailRepo=orderDetailRepository;
            _productRepo=productRepository;
            _cateRepo=categoryRepository;
        }

        public IMemberRepository MemberRepository => _memberRepo;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepo;

        public IOrderRepository OrderRepository => _orderRepo;

        public IProductRepository ProductRepository => _productRepo;

        public ICategoryRepository CategoryRepository => _cateRepo;

        public async Task<bool> SaveChangeAsync()
        {
           return  await _db.SaveChangesAsync()>0;
        }
    }
}
