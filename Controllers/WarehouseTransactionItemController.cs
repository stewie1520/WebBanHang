using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using System.Threading.Tasks;

using WebBanHang.Services.WarehouseTransactionItem;
using WebBanHang.DTOs.WarehouseTransactionItems;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("warehouseTransaction/{warehouseTransactionId}/items")]
    public class WarehouseTransactionItemController : ControllerBase
    {
        private readonly IWarehouseTransactionItemService _service;

        public WarehouseTransactionItemController(IWarehouseTransactionItemService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddItemToTransaction([FromRoute] int warehouseTransactionId, [FromBody] AddWarehouseItemDto dto)
        {
            var res = await _service.AddItemToTransactionAsync(warehouseTransactionId, dto);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpDelete("")]
        public async Task<IActionResult> RemoveItemFromTransaction([FromRoute] int warehouseTransactionId, [FromBody] RemoveWarehouseItemDto dto)
        {
            var res = await _service.RemoveItemFromTransactionAsync(warehouseTransactionId, dto);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }
    }
}
