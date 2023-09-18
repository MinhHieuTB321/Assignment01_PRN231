using AutoMapper;
using BusinessObject.IRepo;
using BusinessObject.Models;
using DataAccess.Dtos.ProductDto;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ProductReadDto> Create(ProductCreateDto productCreateDto)
        {
            if(productCreateDto == null)throw new ArgumentNullException(nameof(productCreateDto));
            //Get max Id
            var maxId = (await _unitOfWork.ProductRepository.GetAll()).Max(x => x.ProductId);
            //Create
            var product = _mapper.Map<Product>(productCreateDto);
            product.ProductId = maxId+1;
            var result = await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.FindByField(x=>x.ProductId == id);
            if (product == null) throw new ArgumentNullException(nameof(product));
            _unitOfWork.ProductRepository.SoftRemove(product);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<List<ProductReadDto>> GetAllProduct()
        {
            var products = await _unitOfWork.ProductRepository.GetAll(x=>x.Category!);
            return _mapper.Map<List<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.FindByField(x=>x.ProductId==id,x=>x.Category!);
            if(product==null) throw new ArgumentNullException(nameof(product));
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<bool> Update(ProductUpdateDto productUpdateDto)
        {
            var product = await _unitOfWork.ProductRepository.FindByField(x => x.ProductId == productUpdateDto.ProductId);
            if (product == null) throw new ArgumentNullException(nameof(product));
            product= _mapper.Map(productUpdateDto,product);
            _unitOfWork.ProductRepository.Update(product);
            return await _unitOfWork.SaveChangeAsync();
        }
    }
}
