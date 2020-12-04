using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Roles;

namespace WebBanHang.DTOs.User
{
    public class GetUserDto
    {
        public string Name { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public List<GetRoleDto> Roles { get; set; }
    }
}
