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
    public class FileUploaderService : BaseService, IFileUploaderService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileUploaderService> _logger;

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

        public bool Validate(Stream inputStream, string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !_fileSignature.Keys.Contains(ext))
            {
                return false;
            }

            long originalPosition = inputStream.Position;
            inputStream.Position = 0;

            var signatures = _fileSignature[ext];
            int maxHeaderBytesLength = signatures.Max(m => m.Length);

            byte[] headerBytes = new byte[maxHeaderBytesLength];
            int offset = 0;
            int remaining = headerBytes.Length;
            while (remaining > 0)
            {
                int read = inputStream.Read(headerBytes, offset, remaining);
                if (read <= 0)
                    break;
                remaining -= read;
                offset += read;
            }

            inputStream.Position = originalPosition;

            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }
}
