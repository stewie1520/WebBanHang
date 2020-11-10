using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DTOs.User;
using WebBanHang.Models;

namespace WebBanHang.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto userRegister);
        Task<ServiceResponse<string>> Login(UserLoginDto userLogin);
        Task<bool> UserExists(string email);
    }
}
