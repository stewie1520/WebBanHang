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
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                }
            }
        };

        private readonly IFileUploaderService _service;

        public UploadController(IFileUploaderService service)
        {
            _service = service;
        }

        [HttpPost("/")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            using var memoryStream = new MemoryStream((int)file.Length);
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var response = await _service.UploadImageAsync(memoryStream, file.FileName, file.Length, file.ContentType);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
