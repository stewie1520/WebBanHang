using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Baskets;
using WebBanHang.DTOs.BasketItems;
using WebBanHang.DTOs.Products;

namespace WebBanHang.Services.Baskets
{
     public interface IBasketItemsService
    {
        Task<ServiceResponse<GetBasketItemDto>> CreateBasketItemAsync(CreateBasketItemDto basketItem, Basket basket);
    }
}