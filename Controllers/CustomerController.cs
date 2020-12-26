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
using WebBanHang.Services.Customers;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private IAuthorizationService<Customer> _customerAuth;
        private ICustomersService _service;
        public CustomerController(IAuthorizationService<Customer> customerAuth, ICustomersService service)
        {
            _customerAuth = customerAuth;
            _service = service;
        }
        // service
        

        

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
        // [HttpPost("")]
        // public async Task<IActionResult> Create([FromBody] CreateCustomerDto customer)
        // {
        //     var response = await _service.CreateCustomerAsync(customer);

        //     if (!response.Success)
        //     {
        //         return BadRequest(response);
        //     }

        //     return Ok(response);
        // }
    }

}
