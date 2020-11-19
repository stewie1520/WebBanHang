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
using WebBanHang.Services.Exceptions;
using Microsoft.AspNetCore.Http;

namespace WebBanHang.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        private readonly DataContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProductsAsync(string name, int page, int perpage)
        {
            
            var response = new ServiceResponse<List<GetProductDto>>();
            try
            {
                var productSkip = (page-1) * perpage;
                int TotalPage;
                List<Product> dbProducts = null;
                if (name != null) 
                {
                    dbProducts = await _context.Products
                        .Include(p => p.Images)
                        .Include(p => p.Category)
                        .Where(p => EF.Functions.Contains(p.Name, name))
                        .Skip(productSkip)
                        .Take(perpage)
                        .ToListAsync(); 
                    TotalPage = await _context.Products
                        .Where(p => EF.Functions.Contains(p.Name, name))
                        .CountAsync();
                }
                else
                {
                    dbProducts = await _context.Products
                        .Include(p => p.Images)
                        .Include(p => p.Category)
                        .Skip(productSkip)
                        .Take(perpage)
                        .ToListAsync();
                    TotalPage = await _context.Products
                    .CountAsync();
                }
                var Pagination = new Pagination{
                    CurrentPage = page,
                    TotalPage = (int) Math.Ceiling(1.0m * TotalPage / perpage)
                };

                response.Data = dbProducts.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
                response.Pagination = Pagination;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.PRODUCT_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);

                return response;
            }
            return response;
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

        public async Task<ServiceResponse<GetProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var response = new ServiceResponse<GetProductDto>();

            try
            {
                var dbProduct = await _context.Products.Include(p => p.Category)
                                    .Include(p => p.Images)
                                    .FirstOrDefaultAsync(p => p.Id == updateProductDto.Id);

                if (dbProduct == null)
                {
                    throw new ProductNotFoundException();
                }

                // Processing for category updating
                if (dbProduct.Category.Id != updateProductDto.CategoryId)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updateProductDto.CategoryId);

                    if (category == null)
                    {
                        throw new CategoryNotFoundException();
                    }

                    dbProduct.Category = category;
                }

                // Processing for images updating
                if (!dbProduct.Images.Select(image => image.Url).SequenceEqual(updateProductDto.ImageUrls))
                {
                    // Remove existed images
                    _context.ProductImages.RemoveRange(dbProduct.Images);
                    var newProductImages = updateProductDto.ImageUrls.Select(url => new ProductImage()
                    {
                        Product = dbProduct,
                        Url = url,
                        ProductId = dbProduct.Id,
                    }).ToList();
                    dbProduct.Images = newProductImages;
                }

                try
                {
                    _context.Products.Update(dbProduct);
                    await _context.SaveChangeWithValidationAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new ProductNotFoundException();
                }

                response.Data = _mapper.Map<GetProductDto>(dbProduct);
                return response;
            }
            catch (BaseServiceException ex)
            {
                response.Success = false;
                response.Message = ex.ErrorMessage;
                response.Code = ex.Code;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{ex.GetType()}");
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.PRODUCT_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }
    }
}
