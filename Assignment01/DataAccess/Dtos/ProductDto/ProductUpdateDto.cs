using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.ProductDto
{
    public class ProductUpdateDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string Weight { get; set; } = null!;
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int UnitsInStock { get; set; }

        [Required]
        public int? CategoryId { get; set; }
    }
}
