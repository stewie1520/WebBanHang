﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Services.FileUploader
{
  public interface IFileUploaderService
  {
    Task<ServiceResponse<FileUpload>> UploadImageAsync(Stream inputStream, string fileName, long fileLength, string contentType);
    Task<ServiceResponse<string>> GetPresignedUploadUrlAsync(WebBanHang.DTOs.Uploads.FileInfo fileInfo);
    bool Validate(Stream inputStream, string fileName);
  }
}
