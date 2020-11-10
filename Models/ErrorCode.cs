using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public enum ErrorCode : int
    {
        NONE = 0,
        INVALID_MODEL_STATE = 1,
        AUTH_INCORRECT_EMAIL_PASSWORD = 100,
        // Errors related with authorization service goes between 100 -> 199
        // Add your code here
        AUTH_USER_EXISTED,
        AUTH_UNEXPECTED_ERROR = 199,
    }
}
