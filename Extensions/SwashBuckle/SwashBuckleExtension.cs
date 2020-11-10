using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using WebBanHang.Filters.SwashBuckle;

namespace WebBanHang.Extensions.SwashBuckle
{
    public static class SwashBuckleExtentions
    {
        public static void AddRequiredAuthorizationHeader(this SwaggerGenOptions options)
            => options.OperationFilter<RequiredAuthorizationHeaderOperationFilter>();
    }
}
