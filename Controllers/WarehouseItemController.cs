using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebBanHang.Services.WarehouseItem;
using WebBanHang.DTOs.WarehouseItems;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WarehouseItemController : ControllerBase
  {
    private readonly IWarehouseItemService _service;
    public WarehouseItemController(IWarehouseItemService service)
    {
      _service = service;
    }

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetAllWarehouseTransaction([FromQuery] PaginationParam pagination, [FromQuery] QueryWarehouseItemDto query)
    {
      var res = await _service.GetAllWarehouseItemsAsync(pagination, query);

      if (res.Success)
      {
        return Ok(res);
      }

      return BadRequest(res);
    }
  }
}
