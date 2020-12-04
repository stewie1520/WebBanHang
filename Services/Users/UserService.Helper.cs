using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Services.Users
{
    public partial class UserService
    {
        private (byte[] passwordHashed, byte[] passwordSalt) CreateHashedPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            byte[] passwordHashed = hmac.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(password));

            return (passwordHashed, hmac.Key);
        }
    }
}
