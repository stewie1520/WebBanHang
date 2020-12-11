using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class RefreshToken : BaseModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddDays(1);
    }
}
