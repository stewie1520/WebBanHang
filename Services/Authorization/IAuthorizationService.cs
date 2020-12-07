using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DTOs.User;
using WebBanHang.DTOs.Customers;
using WebBanHang.Models;

namespace WebBanHang.Services.Authorization
{
    public interface IAuthorizationService<T>
    {
        Task<ServiceResponse<int>> Register(CustomerRegisterDto customerRegister);
        Task<ServiceResponse<UserCredentialDto>> Login(UserLoginDto userLogin);
        Task<bool> UserExists(string email);
    }
}
