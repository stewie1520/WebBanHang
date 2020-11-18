using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Services.Products;
using WebBanHang.DTOs.Products;


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
    }
}
