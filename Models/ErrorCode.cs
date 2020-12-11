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
        AUTH_REFRESH_TOKEN_EXPIRED,
        AUTH_UNEXPECTED_ERROR = 199,
        FILE_UPLOAD_ERROR = 200,
        FILE_UPLOAD_EXTENSION_NOT_PERMITTED = 299,
        CATEGORY_UNEXPECTED_ERROR = 300,
        CATEGORY_PARENT_NOT_FOUND,
        CATEGORY_NOT_FOUND,
        PRODUCT_UNEXPECTED_ERROR = 400,
        PRODUCT_PARENT_NOT_FOUND,
        PRODUCT_NOT_FOUND,
        WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR = 600,
        WAREHOUSE_TRANSACTION_NO_ALLOWING_MODIFIED,
        WAREHOUSE_TRANSACTION_NOT_FOUND = 699,
        USER_UNEXPECTED_ERROR = 700,
        USER_NOT_FOUND,
        WAREHOUSE_TRANSACTION_ITEM_UNEXPECTED_ERROR = 800,
        WAREHOUSE_TRANSACTION_ITEM_NOT_FOUND = 899
    }
}
