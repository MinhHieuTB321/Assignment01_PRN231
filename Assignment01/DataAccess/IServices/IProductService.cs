using BusinessObject.IRepo;
using BusinessObject.Models;
using DataAccess.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IServices
{
    public interface IProductService
    {
        Task<List<ProductReadDto>> GetAllProduct();
        Task<ProductReadDto> GetProductById(int id);
        Task<ProductReadDto> Create(ProductCreateDto productCreateDto);
        Task<bool> Update(ProductUpdateDto productUpdateDto);
        Task<bool> Delete(int id);
    }
}
