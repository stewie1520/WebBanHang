using System;
using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class CategoryNotFoundException : BaseServiceException
    {
        public CategoryNotFoundException() : base(ErrorCode.CATEGORY_NOT_FOUND, "Category not found")
        {
        }
    }
}
