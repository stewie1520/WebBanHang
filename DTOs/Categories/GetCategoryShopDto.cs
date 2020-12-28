using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.Categories
{
  public class GetCategoryShopDto
  {
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
  }
}
