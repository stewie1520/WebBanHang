using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class BasketNotFoundException : BaseServiceException
    {
        public BasketNotFoundException() : base(ErrorCode.BASKET_NOT_FOUND_ERROR, "Basket not found")
        {
        }
    }
}