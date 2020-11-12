using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.Commons;

namespace WebBanHang.Services.FileUploader
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileUploaderService> _logger;

        public FileUploaderService(IConfiguration config, ILogger<FileUploaderService> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task<ServiceResponse<FileUpload>> UploadImageAsync(Stream inputStream, string fileName, long fileLength, string contentType)
        {
            var response = new ServiceResponse<FileUpload>();
            try
            {
                fileName = $"{Guid.NewGuid()}{DateTime.Now.ToString("yyyymmddMMss")}{fileName}";
                S3Helper s3 = new S3Helper(_config);
                string url = await s3.UploadImageAsync(inputStream, fileName, fileLength);

                var fileUpload = new FileUpload
                {
                    Name = fileName,
                    ContentType = contentType,
                    Url = url,
                };

                response.Data = fileUpload;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.FILE_UPLOAD_ERROR;

                return response;
            }
        }
    }
}
