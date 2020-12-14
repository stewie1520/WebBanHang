using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WebBanHang.Extensions.DataContext;
using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.DTOs.User;
using WebBanHang.DTOs.Customers;
using WebBanHang.Services.Exceptions;

namespace WebBanHang.Services.Authorization
{
  public partial class AuthorizationService<T> : BaseService, IAuthorizationService<T> where T : class, IIdentity
  {
    private DataContext _context;
    private IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthorizationService<T>> _logger;

    #region Constructor
    public AuthorizationService(DataContext context, IMapper mapper, IConfiguration config, ILogger<AuthorizationService<T>> logger)
    {
      _context = context;
      _mapper = mapper;
      _config = config;
      _logger = logger;
    }
    #endregion

    #region Login
    /// <summary>
    /// Login - Check if user existed and has correct password, generate a token for requested user
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<UserCredentialDto>> Login(UserLoginDto userLogin)
    {
      var response = new ServiceResponse<UserCredentialDto>();

      try
      {
        var user = await _context.Set<T>().FirstOrDefaultAsync(u => u.Email.ToLower() == userLogin.Email);

        if (user == null)
        {
          response.Success = false;
          response.Message = "User or password incorrect";
          response.Code = ErrorCode.AUTH_INCORRECT_EMAIL_PASSWORD;

          return response;
        }

        var isPasswordCorrect = ComparePassword(userLogin.Password, user.Password, user.PasswordSalt);

        if (!isPasswordCorrect)
        {
          response.Success = false;
          response.Message = "User or password incorrect";
          response.Code = ErrorCode.AUTH_INCORRECT_EMAIL_PASSWORD;

          return response;
        }

        var refreshToken = CreateRefreshToken(user.Email);
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangeWithValidationAsync();

        var expiredAt = DateTime.UtcNow.AddMinutes(15);

        response.Data = new UserCredentialDto()
        {
          AccessToken = CreateToken(user, expiredAt),
          RefreshToken = refreshToken.Token,
          ExpiredAt = expiredAt,
          UserInfo = _mapper.Map<GetUserDto>(user),
        };

        return response;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, ex.StackTrace);
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.AUTH_UNEXPECTED_ERROR;

        return response;
      }
    }
    #endregion

    #region Register
    /// <summary>
    /// Register - Create new user
    /// </summary>
    /// <param name="customerRegister"></param>
    /// <returns></returns>
    public async Task<ServiceResponse<int>> Register(CustomerRegisterDto customerRegister)
    {
      var response = new ServiceResponse<int>();

      try
      {
        var customer = _mapper.Map<Customer>(customerRegister);

        bool HasUserExisted = await UserExists(customer.Email);

        if (HasUserExisted)
        {
          response.Success = false;
          response.Message = "User already existed";
          response.Code = ErrorCode.AUTH_USER_EXISTED;

          return response;
        }

        (customer.Password, customer.PasswordSalt) = CreateHashedPassword(customerRegister.Password);
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        response.Data = customer.Id;

        return response;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, ex.StackTrace);
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.AUTH_UNEXPECTED_ERROR;

        return response;
      }
    }
    #endregion

    #region UserExists
    /// <summary>
    /// UserExists - Check if an user has already registed with email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> UserExists(string email)
    {
      try
      {
        return await _context.Customers.AnyAsync(user => user.Email.ToLower() == email);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, ex.StackTrace);
        return false;
      }
    }
    #endregion


    public async Task<ServiceResponse<UserCredentialDto>> RefreshAsync(RefreshTokenDto refreshDto)
    {
      var response = new ServiceResponse<UserCredentialDto>();

      try
      {
        var refreshToken = refreshDto.RefreshToken;
        var email = refreshDto.Email;

        var dbRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                DateTime.Compare(x.ExpiredAt, DateTime.UtcNow) >= 0 &&
                x.Email == email);

        var dbUser = await _context.Set<T>().FirstOrDefaultAsync(x => x.Email == email);

        if (dbRefreshToken == null || dbUser == null)
        {
          throw new RefreshTokenExpiredException();
        }

        _context.RefreshTokens.Remove(dbRefreshToken);
        var newRefreshToken = CreateRefreshToken(email);

        await _context.RefreshTokens.AddAsync(newRefreshToken);
        await _context.SaveChangeWithValidationAsync();

        var expiredAt = DateTime.UtcNow.AddMinutes(15);

        response.Data = new UserCredentialDto()
        {
          AccessToken = CreateToken(dbUser, expiredAt),
          RefreshToken = newRefreshToken.Token,
          ExpiredAt = expiredAt,
          UserInfo = _mapper.Map<GetUserDto>(dbUser),
        };

        return response;

      }
      catch (BaseServiceException ex)
      {
        _logger.LogError(ex.Message, ex.StackTrace);
        response.Success = false;
        response.Message = ex.ErrorMessage;
        response.Code = ex.Code;

        return response;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, ex.StackTrace);
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.AUTH_UNEXPECTED_ERROR;

        return response;
      }
    }
    public async Task<ServiceResponse<UpdateUserPasswordDto>> UpdateUserPasswordAsync(string userEmail, UpdateUserPasswordDto dto)
    {
      var response = new ServiceResponse<UpdateUserPasswordDto>();

      try
      {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
        if (dbUser == null)
        {
          throw new UserNotFoundException();
        }

        bool isMatchPassword = ComparePassword(dto.currentPassword, dbUser.Password, dbUser.PasswordSalt);
        if (!isMatchPassword)
        {
          throw new PasswordMismatchException();
        }

        (dbUser.Password, dbUser.PasswordSalt) = CreateHashedPassword(dto.newPassword);

        _context.Users.Update(dbUser);
        await _context.SaveChangeWithValidationAsync();

        response.Data = dto;
        return response;
      }
      catch (BaseServiceException ex)
      {
        response.Success = false;
        response.Message = ex.ErrorMessage;
        response.Code = ex.Code;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.AUTH_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
