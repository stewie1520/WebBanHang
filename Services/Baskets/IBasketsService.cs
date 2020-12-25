using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Baskets;

namespace WebBanHang.Services.Baskets
{
     public interface IBasketsService
    {
        Task<ServiceResponse<GetBasketDto>> CreateBasketAsync(CreateBasketDto basket);
    }
}