using BusinessObject.Models;
using DataAccess.Dtos.OrderDetailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.OrderDto
{
    public class OrderReadDto
    {
        public int OrderId { get; set; }

        public string? Member { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }

        public List<OrderDetailReadDto>? OrderDetails { get; set; }
    }
}
