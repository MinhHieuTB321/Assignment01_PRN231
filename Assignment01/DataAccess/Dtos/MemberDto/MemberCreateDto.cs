using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.MemberDto
{
    public class MemberCreateDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string CompanyName { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
