using AutoMapper;
using BusinessObject.IRepo;
using DataAccess.Dtos.CategoryDto;
using DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CategoryReadDto>> GetAllCategory()
        {
            var cates= await _unitOfWork.CategoryRepository.GetAll();
            return _mapper.Map<List<CategoryReadDto>>(cates);
        }
    }
}
