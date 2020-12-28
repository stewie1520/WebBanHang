using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.Baskets;
using WebBanHang.Services.Baskets;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketsController : ControllerBase
    {
        private IBasketsService _service;

        public BasketsController(IBasketsService service)
        {
            _service = service;
        }
        // Create new basket 
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateBasketDto basket)
        {
            var response = await _service.CreateBasketAsync(basket);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}