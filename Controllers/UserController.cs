using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.User;
using WebBanHang.Services.Authorization;
using WebBanHang.Filters;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IAuthorizationService _auth;
        public UserController(IAuthorizationService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto userLogin)
        {
            var response = await _auth.Login(userLogin);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto userRegister)
        {
            var response = await _auth.Register(userRegister);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
