using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.User;
using WebBanHang.Services.Authorization;
using WebBanHang.Filters;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IAuthorizationService<User> _userAuth;
        public UserController(IAuthorizationService<User> userAuth)
        {
            _userAuth = userAuth;
        }

        [HttpPost("login")]
        public async Task<ActionResult> UserLogin(UserLoginDto userLogin)
        {
            var response = await _userAuth.Login(userLogin);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
