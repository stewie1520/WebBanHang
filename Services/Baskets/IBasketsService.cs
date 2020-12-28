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
        Task<ServiceResponse<IEnumerable<GetBasketDto>>> GetAllWarehouseTransactionsAsync(PaginationParam pagination, int type = 0);

    }
}