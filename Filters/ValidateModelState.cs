using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebBanHang.Models;


namespace WebBanHang.Filters
{
    public class ValidateModelState
    {
        public static IActionResult InvalidModelState(ActionContext context)
        {
            return new BadRequestObjectResult(new ServiceResponse<string>
            {
                Success = false,
                Message = context.ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).Aggregate((result, message) => result + ", " + message),
                Code = ErrorCode.INVALID_MODEL_STATE,
            });
        }
    }
}
