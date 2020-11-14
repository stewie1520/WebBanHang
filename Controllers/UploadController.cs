using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebBanHang.Services.FileUploader;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace WebBanHang.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IFileUploaderService _service;

        public UploadController(IFileUploaderService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            using var memoryStream = new MemoryStream((int)file.Length);
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            bool isValidFileExt = _service.Validate(memoryStream, file.FileName);

            if (!isValidFileExt) {  

            }

            var response = await _service.UploadImageAsync(memoryStream, file.FileName, file.Length, file.ContentType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
