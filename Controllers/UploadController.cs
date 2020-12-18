using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;

using WebBanHang.Services.FileUploader;
using WebBanHang.Models;
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
    [Authorize]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
      using var memoryStream = new MemoryStream((int)file.Length);
      await file.CopyToAsync(memoryStream);
      memoryStream.Position = 0;

      bool isValidFileExt = _service.Validate(memoryStream, file.FileName);

      if (!isValidFileExt)
      {

      }

      var response = await _service.UploadImageAsync(memoryStream, file.FileName, file.Length, file.ContentType);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);
    }

    [HttpPost("batch")]
    [Authorize]
    public async Task<IActionResult> UploadMultipleImages(List<IFormFile> files)
    {
      if (files.Count > 10)
      {
        return BadRequest(new ServiceResponse<string>()
        {
          Success = false,
          Message = "Chỉ có thể upload tối da 10 hình ảnh cùng lúc",
          Code = ErrorCode.FILE_UPLOAD_QUANTITY_LIMIT_EXCEED
        });
      }

      var tasks = files.Select(file =>
      {
        using var memoryStream = new MemoryStream((int)file.Length);
        file.CopyToAsync(memoryStream).Wait();
        memoryStream.Position = 0;

        bool isValidFileExt = _service.Validate(memoryStream, file.FileName);

        if (!isValidFileExt)
        {

        }

        return _service.UploadImageAsync(memoryStream, file.FileName, file.Length, file.ContentType);
      });

      var uploadResults = await Task.WhenAll(tasks);

      var imageUrls = uploadResults.Select(result => result.Data).Where(data => data != null).Select(data => data.Url);

      return Ok(new ServiceResponse<IEnumerable<string>>()
      {
        Success = true,
        Data = imageUrls
      });
    }

    [HttpPost("sign")]
    [Authorize]
    public async Task<IActionResult> GetPresignedUploadUrl(WebBanHang.DTOs.Uploads.FileInfo fileInfo)
    {
      var response = await _service.GetPresignedUploadUrlAsync(fileInfo);

      if (!response.Success)
      {
        return BadRequest(response);
      }

      return Ok(response);
    }
  }
}
