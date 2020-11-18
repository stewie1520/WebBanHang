using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebBanHang.DTOs.Products;
using WebBanHang.Models;
using WebBanHang.Data;
using WebBanHang.Extensions.DataContext;

namespace WebBanHang.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(DataContext context, ILogger<ProductService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetProductDto>> CreateProductAsync(CreateProductDto newProductDto)
        {
            var response = new ServiceResponse<GetProductDto>();

            try
            {
                var product = _mapper.Map<Product>(newProductDto);

                if (newProductDto.ParentId.HasValue)
                {
                    var parentProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == newProductDto.ParentId.Value);
                    if (parentProduct == null)
                    {
                        response.Success = false;
                        response.Message = "Parent product is not found";
                        response.Code = ErrorCode.PRODUCT_NOT_FOUND;

                        return response;
                    }

                    product.IsVariant = true;
                    product.IsManageVariant = false;
                    product.Parent = parentProduct;
                }

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == newProductDto.CategoryId);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category of product is not found";
                    response.Code = ErrorCode.CATEGORY_NOT_FOUND;

                    return response;
                }

                product.Category = category;


                var productImages = newProductDto.ImageUrls.Select(url => new ProductImage { Url = url, Product = product }).ToList();
                product.Images = productImages;

                _context.Products.Add(product);

                await _context.SaveChangeWithValidationAsync();

                response.Success = true;
                response.Data = _mapper.Map<GetProductDto>(product);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.PRODUCT_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }

        public async Task<ServiceResponse<GetProductDto>> GetOneProductAsync(int productId)
        {
            var response = new ServiceResponse<GetProductDto>();
            try
            {
                var dbProduct = await _context.Products
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (dbProduct == null)
                {
                    response.Success = false;
                    response.Message = "Product not found";
                    response.Code = ErrorCode.PRODUCT_NOT_FOUND;

                    return response;
                }

                response.Data = _mapper.Map<GetProductDto>(dbProduct);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.PRODUCT_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);

                return response;
            }
        }
    }
}
