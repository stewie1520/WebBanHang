using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using Minio.DataModel;
using Microsoft.Extensions.Configuration;

namespace WebBanHang.Commons
{
    public class S3Helper
    {
        private readonly IConfiguration _config;

        public S3Helper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadImageAsync(Stream inputStream, string fileName, long fileLength)
        {
            string endpoint = _config["AppSettings:S3Endpoint"];
            string apiKey = _config["AppSettings:S3ApiKey"];
            string apiSecret = _config["AppSettings:S3ApiSecret"];
            string bucketName = _config["AppSettings:S3BucketName"];

            var minio = new MinioClient(endpoint, apiKey, apiSecret).WithSSL();
            await minio.PutObjectAsync(bucketName, fileName, inputStream, fileLength);

            return $"{_config["AppSettings:S3UrlEndpoint"]}/{bucketName}/{fileName}";
        }
    }
}
