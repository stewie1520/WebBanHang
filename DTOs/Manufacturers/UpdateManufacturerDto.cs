using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.Manufacturers
{
  public class UpdateManufacturerDto
  {
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string Ward { get; set; }
    public string District { get; set; }
    public string Representative { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
  }
}
