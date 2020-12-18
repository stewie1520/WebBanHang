﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Http;

using WebBanHang.DTOs.Products;
using WebBanHang.Models;
using WebBanHang.Data;
using WebBanHang.Extensions.DataContext;
using WebBanHang.Services.Exceptions;

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

        var newWarehouseItem = new WarehouseItem
        {
          Product = product,
          AverageCost = newProductDto.Cost,
          Quantity = newProductDto.Quantity,
        };

        _context.WarehouseItems.Add(newWarehouseItem);

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

    public async Task<ServiceResponse<List<GetProductDto>>> GetAllProductsAsync(string name, int page, int perpage, QueryProductDto query)
    {

      var response = new ServiceResponse<List<GetProductDto>>();
      try
      {
        var productSkip = (page - 1) * perpage;
        int TotalPage;
        List<Product> dbProducts = null;

        var productQuery = _context.Products.AsQueryable();

        if (name != null)
        {
          productQuery = productQuery.Where(p => EF.Functions.Contains(p.Name, name));
        }

        if (query.IsManageVariant.HasValue)
        {
          productQuery = productQuery.Where(p => p.IsManageVariant == query.IsManageVariant.Value);
        }

        if (query.CategoryId.HasValue)
        {
          productQuery = productQuery.Where(p => p.Category.Id == query.CategoryId.Value);
        }

        if (query.IsVariant.HasValue)
        {
          productQuery = productQuery.Where(p => p.IsVariant == query.IsVariant.Value);
        }

        if (query.Status.HasValue)
        {
          productQuery = productQuery.Where(p => p.Status == query.Status.Value);
        }

        TotalPage = await productQuery.CountAsync();
        dbProducts = await productQuery
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Skip(productSkip)
            .Take(perpage).ToListAsync();

        var Pagination = new Pagination
        {
          CurrentPage = page,
          TotalPage = (int)Math.Ceiling(1.0m * TotalPage / perpage)
        };

        response.Data = dbProducts.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
        response.Pagination = Pagination;
      }
      catch (Exception ex)
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
            .Include(p => p.Children).ThenInclude(child => child.Images)
            .Include(p => p.Children).ThenInclude(child => child.Category)
            .Include(p => p.Children).ThenInclude(child => child.Children)
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

        if (dbProduct.IsManageVariant)
        {
          if (updateProductDto.IsManageVariant)
          {
            // We only care about children when IsManageVariant = true
            var newVariantDtos = updateProductDto.Children?.Where(child => child.Id == 0).ToList();
            var newVariants = _mapper.Map<List<Product>>(newVariantDtos);
            foreach (var variant in newVariants)
            {
              variant.Parent = dbProduct;
              variant.Status = dbProduct.Status;
              variant.Category = dbProduct.Category;
              variant.IsManageVariant = false;
              variant.IsVariant = true;
            }
            await _context.Products.AddRangeAsync(newVariants);

            var existedVariantDtos = updateProductDto.Children?.Where(child => child.Id != 0).ToList();
            var existedVariants = _mapper.Map<List<Product>>(existedVariantDtos);

            foreach (var existedVariant in existedVariants)
            {
              existedVariant.Category = dbProduct.Category;
              existedVariant.IsManageVariant = false;
              existedVariant.IsVariant = true;
            }

            _context.Products.UpdateRange(existedVariants);

            var deletedVariants = await _context.Products
              .Include(x => x.Parent)
              .Include(p => p.Images)
              .Include(p => p.Category)
              .Include(p => p.Children).ThenInclude(child => child.Images)
              .Include(p => p.Children).ThenInclude(child => child.Category)
              .Include(p => p.Children).ThenInclude(child => child.Children)
              .Where(x => x.Parent.Id == dbProduct.Id && !existedVariants.Select(exist => exist.Id).Contains(x.Id))
              .ToListAsync();

            foreach (var variant in deletedVariants)
            {
              variant.IsDeleted = true;
            }

            _context.Products.UpdateRange(deletedVariants);
          }
          else
          {
            var deletedVariants = await _context.Products
              .Include(x => x.Parent)
              .Where(x => x.Parent.Id == dbProduct.Id)
              .ToListAsync();

            foreach (var variant in deletedVariants)
            {
              variant.IsDeleted = true;
            }

            _context.Products.UpdateRange(deletedVariants);
          }
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
          // dbProduct.Images = newProductImages;
          await _context.ProductImages.AddRangeAsync(newProductImages);
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
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.PRODUCT_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
