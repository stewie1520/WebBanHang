
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;
using WebBanHang.DTOs.Categories;

namespace WebBanHang.DTOs.Products
{
  public class GetAllProductShopDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public bool Sale { get; set; }
    public int Price { get; set; }
    public int SalePrice { get; set; }
    public List<GetCategoryShopDto> Categories { get; set; }
    public string Thumbnail { get; set; }
    public IEnumerable<ThumbnailVariant> Variants { get; set; }

    public IEnumerable<BadgeItem> Badge { get; set; }

    public class ThumbnailVariant
    {
      public int Id { get; set; }
      public string Thumbnail { get; set; }
    }

    public class BadgeItem
    {
      public string Type { get; set; }
      public string Value { get; set; }
    }
  }
}
