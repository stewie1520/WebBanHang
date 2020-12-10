using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebBanHang.Models;
using WebBanHang.DTOs.Roles;

namespace WebBanHang.DTOs.User
{
    public class GetUserDto
    {
        public Gender Gender { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public List<GetRoleDto> Roles { get; set; }
    }
}
