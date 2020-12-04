using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebBanHang.Services.WarehouseTransaction;
using WebBanHang.DTOs.WarehouseTransactions;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseTransactionController : ControllerBase
    {
        private readonly IWarehouseTransactionService _service;
        public WarehouseTransactionController(IWarehouseTransactionService service)
        {
            _service = service;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> CreateWarehouseTransaction([FromBody] CreateWarehouseTransactionDto dto)
        {
            var res = await _service.CreateWarehouseTransactionAsync(dto);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }

        [HttpGet("{warehouseTransactionId}")]
        public async Task<IActionResult> GetWarehouseTransaction(int warehouseTransactionId)
        {
            var res = await _service.GetWarehouseTransactionAsync(warehouseTransactionId);

            if (res.Success)
            {
                return Ok(res);
            }

            return BadRequest(res);
        }
    }
}
