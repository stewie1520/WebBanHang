using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.DTOs.Categories;
using WebBanHang.Services.Categories;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private ICategoriesService _service;

        public CategoriesController(ICategoriesService service)
        {
            _service = service;
        }


        /// <summary>
        /// Get all categories in store
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllCategoriesAsync();

            if (response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Create a new category in store
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto category)
        {
            var response = await _service.CreateCategoryAsync(category);

            if (response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        /// <summary>
        /// Update an existing category in store
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDto category)
        {
            var response = await _service.UpdateCategoryAsync(category);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete an existing category in store
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var response = await _service.DeleteCategoryAsync(categoryId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
