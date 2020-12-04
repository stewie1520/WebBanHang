using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class UserNotFoundException : BaseServiceException
    {
        public UserNotFoundException() : base(ErrorCode.USER_NOT_FOUND, "User not found")
        {
        }
    }
}
