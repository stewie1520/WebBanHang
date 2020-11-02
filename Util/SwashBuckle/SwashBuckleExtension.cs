using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace WebBanHang.Util.SwashBuckle
{
    public static class SwashBuckleExtentions
    {
        public static void AddRequiredAuthorizationHeader(this SwaggerGenOptions options)
            => options.OperationFilter<RequiredAuthorizationHeaderOperationFilter>();
    }
}