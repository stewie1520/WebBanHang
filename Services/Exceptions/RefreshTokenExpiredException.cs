using System;
using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class RefreshTokenExpiredException : BaseServiceException
    {
        public RefreshTokenExpiredException() : base(ErrorCode.AUTH_REFRESH_TOKEN_EXPIRED, "Refresh token expired")
        {
        }
    }
}
