using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Helpers.AwsS3BucketHelper
{
    public interface IAwsS3BucketHelper
    {
        Task<bool> UploadFile(System.IO.Stream inputStream, string fileName);
    }
}
