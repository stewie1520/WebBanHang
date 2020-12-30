using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.Baskets;
using WebBanHang.DTOs.Commons;
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
    // Get all basket
    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetAllBaskets([FromQuery] PaginationParam pagination, [FromQuery] QueryBasketDto query)
    {
      var res = await _service.GetAllBasketsAsync(pagination, query);

      if (res.Success)
      {
        return Ok(res);
      }

      return BadRequest(res);
    }
    [HttpGet("{basketId}")]
    [Authorize]
    public async Task<IActionResult> GetBasket(int basketId)
    {
      var res = await _service.GetBasketAsync(basketId);

      if (res.Success)
      {
        return Ok(res);
      }

      return BadRequest(res);
    }
    [HttpPut("")]
    [Authorize]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketStatusDto basketStatus)
    {
      var res = await _service.UpdateBasketStatusAsync(basketStatus);

      if (res.Success)
      {
        return Ok(res);
      }

      return BadRequest(res);
    }
  }
}
