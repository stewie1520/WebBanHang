using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Categories;

namespace WebBanHang.Services.Categories
{
    public interface ICategoriesService
    {
        Task<ServiceResponse<GetCategoryDto>> CreateCategoryAsync(CreateCategoryDto category);
        Task<ServiceResponse<List<GetCategoryDto>>> GetAllCategoriesAsync();
        Task<ServiceResponse<GetCategoryDto>> UpdateCategoryAsync(UpdateCategoryDto category);
        Task<ServiceResponse<int>> DeleteCategoryAsync(int categoryId);
    }
}
