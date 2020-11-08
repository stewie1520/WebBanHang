using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class BaseAccount : BaseModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
