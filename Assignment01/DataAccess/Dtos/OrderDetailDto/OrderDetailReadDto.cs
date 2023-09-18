using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.OrderDetailDto
{
    public class OrderDetailReadDto
    {
        public int OrderId { get; set; }

        public string? ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

    }
}
