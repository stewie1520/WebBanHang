﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Products
{
  public class GetProductDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int Price { get; set; }
    public bool IsVariant { get; set; } = false;
    public bool IsManageVariant { get; set; } = false;
    [Required]
    public int CategoryId { get; set; }
    public int? ParentId { get; set; }
    public IEnumerable<int> ChildrenIds { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public IEnumerable<GetProductDto> Children { get; set; }
    public IEnumerable<string> ImageUrls { get; set; }
    public ProductStatus Status { get; set; }
    public string CategoryText { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<string> Features { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }

    public bool IsDiscount { get; set; } = false;
    public int PriceBeforeDiscount { get; set; }

  }
}
