using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.DTOs.User;
using AutoMapper;

namespace WebBanHang.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private DataContext _context;
        private IMapper _mapper;
        public AuthorizationService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> Login(UserLoginDto userLogin)
        {
            var response = new ServiceResponse<string>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userLogin.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User or password incorrect";

                return response;
            }

            var isPasswordCorrect = ComparePassword(userLogin.Password, user.Password, user.PasswordSalt);

            if (!isPasswordCorrect)
            {
                response.Success = false;
                response.Message = "User or password incorrect";

                return response;
            }


            // TODO: return token;
            response.Data = "12345";
            return response;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDto userRegister)
        {
            var response = new ServiceResponse<int>();

            var user = _mapper.Map<User>(userRegister);

            bool HasUserExisted = await UserExists(user.Email);

            if (HasUserExisted)
            {
                response.Success = false;
                response.Message = "User already existed";

                return response;
            }

            (user.Password, user.PasswordSalt) = CreateHashedPassword(userRegister.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            response.Data = user.Id;

            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.ToLower() == email);
        }

        private (byte[] passwordHashed, byte[] passwordSalt) CreateHashedPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            byte[] passwordHashed = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(password));

            return (passwordHashed, hmac.Key);
        }

        private bool ComparePassword(string password, byte[] passwordHashed, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            if (computedHash.Length != passwordHashed.Length)
                return false;

            for (int i = 0; i < passwordHashed.Length; i++)
            {
                if (computedHash[i] != passwordHashed[i])
                    return false;
            }

            return true;
        }
    }
}
