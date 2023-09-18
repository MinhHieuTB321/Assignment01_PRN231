using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.CategoryDto
{
    public class CategoryReadDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}
