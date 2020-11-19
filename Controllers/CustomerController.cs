using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.Customers;
using WebBanHang.DTOs.User;
using WebBanHang.Services.Authorization;
using WebBanHang.Filters;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
            private IAuthorizationService<Customer> _customerAuth;
            public CustomerController(IAuthorizationService<Customer> customerAuth)
            {
                _customerAuth = customerAuth;
            }

            [HttpPost("login")]
            public async Task<ActionResult> CustomerLogin(UserLoginDto customerLogin)
            {
                var response = await _customerAuth.Login(customerLogin);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            [HttpPost("register")]
            public async Task<ActionResult> CustomerRegister(CustomerRegisterDto customerRegister)
            {
                var response = await _customerAuth.Register(customerRegister);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
    }
    
}
