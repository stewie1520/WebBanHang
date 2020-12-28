using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WebBanHang.Data;
using WebBanHang.DTOs.Customers;
using WebBanHang.Extensions.DataContext;
using WebBanHang.Models;

namespace WebBanHang.Services.Customers
{
    public class CustomersService : BaseService, ICustomersService
    {
        private readonly DataContext _context;
        private readonly ILogger<CustomersService> _logger;
        private readonly IMapper _mapper;

        public CustomersService(DataContext context, IMapper mapper, ILogger<CustomersService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        
        // Tạo customer
        public async Task<ServiceResponse<GetCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var response = new ServiceResponse<GetCustomerDto>();
            try {
                var customer = _mapper.Map<Customer>(createCustomerDto);

                var address = _mapper.Map<Address>(createCustomerDto.Address);

                await _context.Customers.AddAsync(customer);

                await _context.Addresses.AddAsync(address);
                
                address.Customer = customer;

                // customer.Addresses.Append(address);

                await _context.SaveChangeWithValidationAsync();

                var getCustomerDto = _mapper.Map<GetCustomerDto>(customer);

                response.Data = getCustomerDto;

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