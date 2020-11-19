﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Services.Products;
using WebBanHang.DTOs.Products;
using WebBanHang.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _services;

        public ProductController(IProductService service)
        {
            _services = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto newProductDto)
        {
            var response = await _services.CreateProductAsync(newProductDto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetOne(int productId)
        {
            var response = await _services.GetOneProductAsync(productId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update an existing products
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        [HttpPut("")]
        [SwaggerResponse(200, "Update successfully", typeof(ServiceResponse<GetProductDto>))]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            var response = await _services.UpdateProductAsync(updateProductDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}