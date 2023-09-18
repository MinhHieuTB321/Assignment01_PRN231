
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IRepo
{
    public interface IUnitOfWork
    {
        public IMemberRepository MemberRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public Task<bool> SaveChangeAsync();
    }
}
