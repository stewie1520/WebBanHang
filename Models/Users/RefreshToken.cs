using System;

namespace WebBanHang.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
