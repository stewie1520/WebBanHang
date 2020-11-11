using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Helpers.AwsS3BucketHelper
{
    public class AwsS3BucketHelper : IAwsS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;
        public AwsS3BucketHelper(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        public async Task<bool> UploadFile(Stream inputStream, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = "",
                    Key = fileName,
                };

                PutObjectResponse response = await _amazonS3.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
