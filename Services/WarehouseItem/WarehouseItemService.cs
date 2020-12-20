using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using WebBanHang.Services.Exceptions;
using WebBanHang.Models;
using WebBanHang.Data;
using WebBanHang.DTOs.WarehouseItems;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.WarehouseItem
{
  public class WarehouseItemService : IWarehouseItemService
  {
    private readonly DataContext _context;
    private readonly ILogger<WarehouseItemService> _logger;
    private readonly IMapper _mapper;
    public WarehouseItemService(DataContext context, ILogger<WarehouseItemService> logger, IMapper mapper)
    {
      _context = context;
      _logger = logger;
      _mapper = mapper;
    }
    public async Task<ServiceResponse<IEnumerable<GetWarehouseItemDto>>> GetAllWarehouseItemsAsync(PaginationParam pagination, QueryWarehouseItemDto query)
    {
      var response = new ServiceResponse<IEnumerable<GetWarehouseItemDto>>();
      try
      {
        var dbWarehouseItems = await _context.WarehouseItems
          .Include(x => x.Product).ThenInclude(p => p.Images)
          .Include(x => x.Product).ThenInclude(p => p.Category)
          .Take(pagination.PerPage)
          .Skip(pagination.Skip())
          .ToListAsync();

        var totalItemsQuantity = await _context.WarehouseItems
          .CountAsync();

        // response.Data = _mapper.Map<IEnumerable<GetWarehouseItemDto>>(dbWarehouseItems); 
        response.Data = dbWarehouseItems.Select(item => _mapper.Map<GetWarehouseItemDto>(item)); 
        response.Pagination = new Pagination
        {
          CurrentPage = pagination.Page,
          TotalPage = pagination.TotalPage(totalItemsQuantity)
        };

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
        response.Code = ErrorCode.WAREHOUSE_ITEM_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
