using DataAccess.Dtos.OrderDetailDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.OrderDto
{
    public class OrderCreateDto
    {
        [Required]
        public int? MemberId { get; set; }
        [Required]
        public DateTime? OrderDate { get; set; }
        [Required]
        public DateTime? RequiredDate { get; set; }
        [Required]
        public DateTime? ShippedDate { get; set; }
        [Required]
        public decimal? Freight { get; set; }
        [Required]
        public List<OrderDetailCreateDto>? OrderDetails { get; set; }
    }

    
}
