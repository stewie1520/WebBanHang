using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.User
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
