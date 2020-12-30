using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Baskets;
using System.Collections.Generic;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.Baskets
{
     public interface IBasketsService
    {
        Task<ServiceResponse<GetBasketDto>> CreateBasketAsync(CreateBasketDto basket);
        Task<ServiceResponse<IEnumerable<GetBasketWithoutItemDto>>> GetAllBasketsAsync(PaginationParam pagination, int type = 0);

        Task<ServiceResponse<GetBasketDto>> GetBasketAsync(int basketId);
        Task<ServiceResponse<IEnumerable<GetBasketDto>>> GetAllBasketsByCustomerAsync(PaginationParam pagination, int type = 0);
        Task<ServiceResponse<GetBasketDto>> GetBasketByCustomerAsync(int basketId);
        Task<ServiceResponse<GetBasketDto>> UpdateBasketStatusAsync(UpdateBasketStatusDto basket);

    }
}
