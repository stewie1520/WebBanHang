using System;
using System.ComponentModel.DataAnnotations;

using WebBanHang.Models;

namespace WebBanHang.DTOs.User
{
  public class UpdateUserDto
  {
    [Required]
    public string Avatar { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Address { get; set; }
  }
}
