using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebBanHang.Data;
using WebBanHang.DTOs.BasketItems;
using WebBanHang.DTOs.Baskets;
using WebBanHang.DTOs.Products;
using WebBanHang.Extensions.DataContext;
using WebBanHang.Models;
using WebBanHang.Services.Baskets;

namespace WebBanHang.Services.BasketItems
{
    public class BasketItemsService : BaseService, IBasketItemsService
    {
        private readonly DataContext _context;
        private readonly ILogger<BasketsService> _logger;
        private readonly IMapper _mapper;
        public BasketItemsService(DataContext context, IMapper mapper, ILogger<BasketsService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<GetBasketItemDto>> CreateBasketItemAsync(CreateBasketItemDto createBasketItemDto, int basketId)
        {
            var response = new ServiceResponse<GetBasketItemDto>();
            try {
                var basketItem = _mapper.Map<BasketItem>(createBasketItemDto);

                await _context.BasketItems.AddAsync(basketItem);

                var product = _context.Products
                    .Include(x => x.Category)
                    .FirstOrDefault(c => c.Id == createBasketItemDto.ProductId);

                // var category = _mapper.Map<Category>(product.Category);

                var basket = _context.Baskets.FirstOrDefault(c => c.Id == basketId);

                basketItem.Product = product;

                basketItem.Basket = basket;

                basketItem.Price = product.Price * basketItem.Quantity;

                await _context.SaveChangeWithValidationAsync();

                var getBasketItemDto = _mapper.Map<GetBasketItemDto>(basketItem);

                response.Data = getBasketItemDto;

                return response;
            } 
            catch (Exception ex){
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.NONE;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }
    }
}


