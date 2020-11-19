using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBanHang.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebBanHang.Services.Authorization
{
    public partial class AuthorizationService<T>
    {
        #region CreateHashedPassword
        /// <summary>
        /// CreateHashedPassword - Create an hashed password from raw password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hashed password and password salt</returns>
        private (byte[] passwordHashed, byte[] passwordSalt) CreateHashedPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            byte[] passwordHashed = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(password));

            return (passwordHashed, hmac.Key);
        }
        #endregion

        #region ComparePassword
        /// <summary>
        /// Hash a raw password then compare it with another hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHashed"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>true if hashing raw password return the same as passwordHased</returns>
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
        #endregion

        #region CreateToken
        /// <summary>
        /// Generate a token from user info
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(T user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Email}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, typeof(T).Name),
            };

            var key = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(_config["AppSettings:SecretKey"]));

            var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = signingCredential,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
