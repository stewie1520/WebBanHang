using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.DTOs.User
{
    public class CreateUserDto
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public string Password { get; set; }
        public IEnumerable<int> Roles { get; set; }
    }
}
