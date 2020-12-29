
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;
using WebBanHang.DTOs.Categories;
using static WebBanHang.DTOs.Products.GetAllProductShopDto;

namespace WebBanHang.DTOs.Products
{
  public class GetProductShopDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public bool Sale { get; set; }
    public int Price { get; set; }
    public ProductStatus Status { get; set; }
    public int SalePrice { get; set; }

    public string Description { get; set; }

    public bool IsVariant { get; set; } = false;
    public bool IsManageVariant { get; set; } = false;
    public List<GetProductShopDto> Children { get; set; }
    public List<GetCategoryShopDto> Categories { get; set; }
    public string Thumbnail { get; set; }
    public IEnumerable<ThumbnailVariant> Variants { get; set; }

    public IEnumerable<BadgeItem> Badge { get; set; }
    public List<string> Features { get; set; }
    public List<string> Tags { get; set; }
  }
}
