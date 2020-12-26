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
using WebBanHang.Services.Customers;
using WebBanHang.DTOs.Customers;

namespace WebBanHang.Services.Baskets
{
    public class BasketsService : BaseService, IBasketsService
    {
        private readonly DataContext _context;
        private readonly ILogger<BasketsService> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomersService _customerService;
        public BasketsService(DataContext context, IMapper mapper, ILogger<BasketsService> logger, ICustomersService customerService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }
        public async Task<ServiceResponse<GetBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto)
        {
            var response = new ServiceResponse<GetBasketDto>();
            try 
            {
                var basket = _mapper.Map<Basket>(createBasketDto);
                // Check for customer if email exist
                if (createBasketDto.Email != null){

                    var customer = _context.Customers.FirstOrDefault(c => c.Email == createBasketDto.Email);
                    if (customer == null)
                    {
                        // Tạo khách hàng (bỏ sau)
                        var customerDto = new CreateCustomerDto
                        {
                            Email = createBasketDto.Email,
                            FullName = createBasketDto.FullName,
                            Gender = Gender.Unknown,
                            Address = createBasketDto.Address
                        };
                        var createNewCustomerResult = await _customerService.CreateCustomerAsync(customerDto);
                        if (!createNewCustomerResult.Success) {
                            response.Success = false;
                            response.Message = createNewCustomerResult.Message;
                            
                            return response;
                        }
                        customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == response.Data.Id);
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