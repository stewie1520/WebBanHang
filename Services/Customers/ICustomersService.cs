using System.Threading.Tasks;
using System.Collections.Generic;
using WebBanHang.DTOs.Customers;
using WebBanHang.Models;

namespace WebBanHang.Services.Customers
{
    public interface ICustomersService
    {
        Task<ServiceResponse<GetCustomerDto>> CreateCustomerAsync(CreateCustomerDto customer);
        // Task<ServiceResponse<GetAddressDto>> CreateAddressAsync(CreateAddressDto address);
    }
}