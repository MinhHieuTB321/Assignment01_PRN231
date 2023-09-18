using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.AuthenticationDto
{
    public class LoginResponseDto
    {
        public int MemberId {  get; set; }
        public string? AccessToken {  get; set; }
        public string? Role {  get; set; }
    }
}
