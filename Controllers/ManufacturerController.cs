using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebBanHang.DTOs.Manufacturers;
using WebBanHang.DTOs.Commons;
using WebBanHang.Services.Manufacturers;

namespace WebBanHang.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ManufacturerController : ControllerBase
  {
    private IManufacturerService _service;

    public ManufacturerController(IManufacturerService service)
    {
      _service = service;
    }

    /// <summary>
    /// Create a new manufacturer in store
    /// </summary>
    /// <param name="newCategoryDto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateManufacturerDto newCategoryDto)
    {
      var response = await _service.CreateManufacturerAsync(newCategoryDto);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);

    }

    /// <summary>
    /// Update a new manufacturer in store
    /// </summary>
    /// <param name="categoryDto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] UpdateManufacturerDto categoryDto)
    {
      var response = await _service.UpdateManufacturerAsync(categoryDto);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);

    }

    /// <summary>
    /// Get a new manufacturer in store
    /// </summary>
    /// <param name="manufacturerId"></param>
    /// <returns></returns>
    [HttpGet("{manufacturerId}")]
    public async Task<IActionResult> GetOne(int manufacturerId)
    {
      var response = await _service.GetManufacturerAsync(manufacturerId);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);

    }


    /// <summary>
    /// Get all manufacturers in store
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParam pagination, [FromQuery] QueryManufacturerDto query)
    {
      var response = await _service.GetAllManufacturerAsync(pagination, query);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);

    }

    /// <summary>
    /// Delete a manufacturer in store
    /// </summary>
    /// <param name="manufacturerId"></param>
    /// <returns></returns>
    [HttpDelete("{manufacturerId}")]
    public async Task<IActionResult> DeleteOne(int manufacturerId)
    {
      var response = await _service.DeleteManufacturerAsync(manufacturerId);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);

    }

  }
}
