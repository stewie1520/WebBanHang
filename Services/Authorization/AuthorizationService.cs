using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using WebBanHang.Extensions.DataContext;
using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.DTOs.User;
using WebBanHang.DTOs.Customers;

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
        public async Task<ServiceResponse<string>> Login(UserLoginDto userLogin)
        {
            var response = new ServiceResponse<string>();

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

                string accessToken = CreateToken(user);

                response.Data = accessToken;
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
    }
}
