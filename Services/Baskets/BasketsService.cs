using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.DTOs.Baskets;
using WebBanHang.Models;
using WebBanHang.Extensions.DataContext;
using System.Linq;

namespace WebBanHang.Services.Baskets
{
    public class BasketsService : BaseService, IBasketsService
    {
        private readonly DataContext _context;
        private readonly ILogger<BasketsService> _logger;
        private readonly IMapper _mapper;
        public BasketsService(DataContext context, IMapper mapper, ILogger<BasketsService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<GetBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto)
        {
            var response = new ServiceResponse<GetBasketDto>();
            try 
            {
                var basket = _mapper.Map<Basket>(createBasketDto);
                // Check for customer if customerId exist
                if (createBasketDto.CustomerId != null){

                    var customer = _context.Customers.FirstOrDefault(c => c.Id == createBasketDto.CustomerId.Value);
                    if (customer == null){
                        response.Success = false;
                        response.Message = "Customer id is not found";
                        response.Code = ErrorCode.NONE;

                        return response;
                    }
                    basket.Customer = customer;
                }


                _context.Baskets.Add(basket);

                await _context.SaveChangeWithValidationAsync();

                var getBasketDto = _mapper.Map<GetBasketDto>(basket);

                response.Data = getBasketDto;

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