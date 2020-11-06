using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class User : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public UserType Type { get; set; } = UserType.CommonUser;
        public IEnumerable<Address> Addresses { get; set; }
    }
}
