using System;
using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class ProductNotFoundException : BaseServiceException
    {
        public ProductNotFoundException() : base(ErrorCode.PRODUCT_NOT_FOUND, "Product not found")
        {
        }
    }
}
