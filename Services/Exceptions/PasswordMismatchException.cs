using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
  public class PasswordMismatchException : BaseServiceException
  {
    public PasswordMismatchException() : base(ErrorCode.AUTH_PASSWORD_MISMATCH, "Current password is incorrect")
    {
    }
  }
}
