using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Products;

namespace WebBanHang.Services.Products
{
  public interface IProductService
  {
    Task<ServiceResponse<GetProductDto>> CreateProductAsync(CreateProductDto newProductDto);
    Task<ServiceResponse<GetProductDto>> GetOneProductAsync(int productId);
    Task<ServiceResponse<List<GetProductDto>>> GetAllProductsAsync(string name, int page, int perpage, QueryProductDto query);
    Task<ServiceResponse<List<GetAllProductShopDto>>> GetAllProductsShopAsync(string name, int page, int perpage, QueryProductDto query);
    Task<ServiceResponse<GetProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto);
    Task<ServiceResponse<int>> DeleteProductAsync(int productId);
  }
}
