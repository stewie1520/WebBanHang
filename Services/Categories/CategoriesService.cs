using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebBanHang.DTOs.Categories;
using WebBanHang.Models;
using WebBanHang.Data;
using WebBanHang.Extensions.DataContext;

namespace WebBanHang.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _context;
        private readonly ILogger<CategoriesService> _logger;
        private readonly IMapper _mapper;

        public CategoriesService(DataContext context, IMapper mapper, ILogger<CategoriesService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<GetCategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            try
            {
                var category = _mapper.Map<Category>(createCategoryDto);
                // Check for parent category if category.ParentId exist
                if (createCategoryDto.ParentId != null)
                {
                    var parentCategory = _context.Categories.FirstOrDefault(c => c.Id == createCategoryDto.ParentId.Value);
                    if (parentCategory == null)
                    {
                        response.Success = false;
                        response.Message = "Parent category is not found";
                        response.Code = ErrorCode.CATEGORY_PARENT_NOT_FOUND;

                        return response;
                    }

                    category.Parent = parentCategory;
                }


                _context.Categories.Add(category);

                await _context.SaveChangeWithValidationAsync();

                var getCategoryDto = _mapper.Map<GetCategoryDto>(category);

                response.Data = getCategoryDto;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.CATEGORY_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }

        public async Task<ServiceResponse<int>> DeleteCategoryAsync(int categoryId)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var categories = await _context.Categories
                    .Where(c => c.Id == categoryId || c.Parent.Id == categoryId)
                    .Include(c => c.Parent)
                    .ToListAsync();

                var deletedCategories = categories.Select(c =>
                {
                    c.IsDeleted = true;
                    return c;
                });

                _context.Categories.UpdateRange(deletedCategories);
                await _context.SaveChangeWithValidationAsync();

                response.Data = categoryId;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.CATEGORY_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }

        public async Task<ServiceResponse<List<GetCategoryDto>>> GetAllCategoriesAsync()
        {
            var response = new ServiceResponse<List<GetCategoryDto>>();

            try
            {
                var categories = await _context.Categories.ToListAsync();

                var getCategoryDtos = categories.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();

                response.Success = true;
                response.Data = getCategoryDtos;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.CATEGORY_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);

                return response;
            }
        }

        public async Task<ServiceResponse<GetCategoryDto>> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var response = new ServiceResponse<GetCategoryDto>();

            try
            {
                var category = await _context.Categories
                                        .Include(c => c.Parent)
                                        .FirstOrDefaultAsync(c => c.Id == updateCategoryDto.Id);

                if (category == null)
                {
                    response.Success = false;
                    response.Message = "Category is not found";
                    response.Code = ErrorCode.CATEGORY_NOT_FOUND;

                    return response;
                }

                // Check for parent category if category.ParentId exist
                if (updateCategoryDto.ParentId != null && updateCategoryDto.ParentId.Value != category.Parent.Id)
                {
                    var parentCategory = _context.Categories.FirstOrDefault(c => c.Id == updateCategoryDto.ParentId.Value);
                    if (parentCategory == null)
                    {
                        response.Success = false;
                        response.Message = "Parent category is not found";
                        response.Code = ErrorCode.CATEGORY_PARENT_NOT_FOUND;

                        return response;
                    }

                    category.Parent = parentCategory;
                }

                category.Name = updateCategoryDto.Name;
                category.Tier = updateCategoryDto.Tier;

                _context.Categories.Update(category);
                await _context.SaveChangeWithValidationAsync();

                var getCategoryDto = _mapper.Map<GetCategoryDto>(category);

                response.Data = getCategoryDto;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ErrorCode.CATEGORY_UNEXPECTED_ERROR;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }
    }
}

