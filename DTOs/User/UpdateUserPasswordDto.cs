using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.User
{
  public class UpdateUserPasswordDto
  {
    [Required]
    public string currentPassword { get; set; }
    [Required]
    public string newPassword { get; set; }
  }
}
