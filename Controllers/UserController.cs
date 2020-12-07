using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebBanHang.DTOs.User;
using WebBanHang.Services.Authorization;
using WebBanHang.Services.Users;
using WebBanHang.Filters;
using WebBanHang.Models;
using WebBanHang.Services;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IAuthorizationService<User> _userAuth;
        public UserController(IAuthorizationService<User> userAuth, IUserService service)
        {
            _service = service;
            _userAuth = userAuth;
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        /// <param name="userLogin"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /login
        ///     {
        ///         "email": "donghuuhieu1520@gmail.com",
        ///         "password": "123456"
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> UserLogin(UserLoginDto userLogin)
        {
            var response = await _userAuth.Login(userLogin);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllUsers(int page = 1, int perpage = BaseService.DefaultPerPage)
        {
            var response = await _service.GetAllUsers(page, perpage);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserDto newUser)
        {
            var response = await _service.CreateUser(newUser);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
