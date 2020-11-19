using System;
using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public abstract class BaseServiceException : ApplicationException
    {
        public readonly ErrorCode Code;
        public readonly string ErrorMessage;
        public BaseServiceException(ErrorCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }
    }
}
