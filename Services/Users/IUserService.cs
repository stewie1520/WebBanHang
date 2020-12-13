using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.User;

namespace WebBanHang.Services.Users
{
  public interface IUserService
  {
    Task<ServiceResponse<List<GetUserDto>>> GetAllUsers(int page, int perpage);
    Task<ServiceResponse<GetUserDto>> CreateUser(CreateUserDto newUser);
    Task<ServiceResponse<GetUserDto>> GetUserAsync(string userEmail);
    Task<ServiceResponse<GetUserDto>> UpdateUserAsync(string userEmail, UpdateUserDto dto);
  }
}
