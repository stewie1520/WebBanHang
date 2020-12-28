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
using System.Collections.Generic;
using WebBanHang.DTOs.BasketItems;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.Baskets
{
    public class BasketsService : BaseService, IBasketsService
    {
        private readonly DataContext _context;
        private readonly ILogger<BasketsService> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomersService _customerService;
        private readonly IBasketItemsService _basketItemService;
        public BasketsService(DataContext context, IMapper mapper, ILogger<BasketsService> logger, ICustomersService customerService, IBasketItemsService basketItemService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
            _basketItemService = basketItemService;
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
                // Add basket item
                
                _context.Baskets.Add(basket);

                await _context.SaveChangeWithValidationAsync();

                var getBasketDto = _mapper.Map<GetBasketDto>(basket);

                foreach (CreateBasketItemDto createBasketItem in createBasketDto.Items){
                    await _basketItemService.CreateBasketItemAsync(createBasketItem, basket.Id);

                    // if (!createNewBasketItemResult.Success) {

                    //     response.Success = false;

                    //     response.Message = createNewBasketItemResult.Message;
                        
                    //     return response;
                    // }
                }
                 
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

        // public Task<ServiceResponse<IEnumerable<GetBasketDto>>> GetAllWarehouseTransactionsAsync(PaginationParam pagination, int type = 0)
        // {
            
        // }
    }
}