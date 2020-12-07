using System;

namespace WebBanHang.DTOs.User
{
    public class UserCredentialDto
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
