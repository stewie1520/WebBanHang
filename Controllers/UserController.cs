using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserController(IAuthorizationService<User> userAuth, IUserService service, IHttpContextAccessor httpContextAccessor)
    {
      _service = service;
      _userAuth = userAuth;
      _httpContextAccessor = httpContextAccessor;
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

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyInfo()
    {
      string userIndentifier = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userIndentifier == null)
      {
        return Forbid();
      }

      var response = await _service.GetUserAsync(userIndentifier);
      if (response.Success)
      {
        return Ok(response);
      }

      return BadRequest(response);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMyInfo([FromBody] UpdateUserDto dto)
    {
      string userIndentifier = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userIndentifier == null)
      {
        return Forbid();
      }

      var response = await _service.UpdateUserAsync(userIndentifier, dto);
      if (response.Success)
      {
        return Ok(response);
      }

      return BadRequest(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshDto)
    {
      var res = await _userAuth.RefreshAsync(refreshDto);

      if (res.Success)
      {
        return Ok(res);
      }

      return BadRequest(res);
    }
  }
}
