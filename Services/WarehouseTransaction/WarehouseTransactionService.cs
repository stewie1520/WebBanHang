using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.Services.Exceptions;
using WebBanHang.Extensions.DataContext;
using WebBanHang.Data;
using WebBanHang.Commons;

namespace WebBanHang.Services.WarehouseTransaction
{
  using WebBanHang.DTOs.Commons;
  using WebBanHang.Models;

  public partial class WarehouseTransactionService : IWarehouseTransactionService
  {
    private readonly ILogger<WarehouseTransactionService> _logger;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpCtxAccessor;

    public WarehouseTransactionService(ILogger<WarehouseTransactionService> logger, IMapper mapper,
                                       DataContext context, IHttpContextAccessor httpCtxAccessor)
    {
      _logger = logger;
      _mapper = mapper;
      _context = context;
      _httpCtxAccessor = httpCtxAccessor;
    }

    public async Task<ServiceResponse<GetWarehouseTransactionDto>> CreateWarehouseTransactionAsync(CreateWarehouseTransactionDto dto)
    {
      var response = new ServiceResponse<GetWarehouseTransactionDto>();

      try
      {
        var userIdentifier = _httpCtxAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userIdentifier);

        if (user == null)
        {
          throw new UserNotFoundException();
        }

        Manufacturer dbManufacturer = null;

        if (dto.TransactionType == WarehouseTransactionType.Import)
        {
          dbManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == dto.ManufacturerId);
          if (dbManufacturer == null)
          {
            throw new ManufacturerNotFoundException();
          }
        }

        var newTransaction = new WarehouseTransaction()
        {
          TransactionType = dto.TransactionType,
          CreatedAt = dto.CreatedAt,
          Status = WarehouseTransactionStatus.Processing, // new warehouse transaction has processing status as default
          Manufacturer = dbManufacturer,
          CreatedBy = user,
          Description = dto.Description,
        };

        await _context.WarehouseTransactions.AddAsync(newTransaction);
        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetWarehouseTransactionDto>(newTransaction);

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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetWarehouseTransactionWithoutItemDto>> DeleteWarehouseTransactionAsync(int warehouseTransactionId)
    {
      var response = new ServiceResponse<GetWarehouseTransactionWithoutItemDto>();

      try
      {
        var dbWarehouseTransaction = await _context.WarehouseTransactions.FirstOrDefaultAsync(x => x.Id == warehouseTransactionId);

        if (dbWarehouseTransaction == null)
        {
          throw new WarehouseTransactionNotFoundException();
        }

        if (!dbWarehouseTransaction.CanModify())
        {
          throw new WarehouseTransactionModifiedException();
        }


        dbWarehouseTransaction.IsDeleted = true;
        _context.WarehouseTransactions.Update(dbWarehouseTransaction);

        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetWarehouseTransactionWithoutItemDto>(dbWarehouseTransaction);

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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<List<GetManufacturerDto>>> GetAllManufacturersAsync()
    {
      var response = new ServiceResponse<List<GetManufacturerDto>>();

      try
      {
        var dbManufacturers = await _context.Manufacturers.ToListAsync();
        response.Data = _mapper.Map<List<GetManufacturerDto>>(dbManufacturers);
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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<List<GetProductDto>>> GetAllProductsAsync()
    {
      var response = new ServiceResponse<List<GetProductDto>>();

      try
      {
        var dbProducts = await _context.Products
          .Include(p => p.Images)
          .ToListAsync();
        response.Data = dbProducts.Select(x => _mapper.Map<GetProductDto>(x)).ToList();
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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<IEnumerable<GetWarehouseTransactionWithoutItemDto>>> GetAllWarehouseTransactionsAsync(PaginationParam pagination, int type = 0)
    {
      var response = new ServiceResponse<IEnumerable<GetWarehouseTransactionWithoutItemDto>>();

      try
      {
        var warehouseTransactionType = (WarehouseTransactionType)type;

        var dbWarehouseTransactions = await _context.WarehouseTransactions
            .Include(x => x.Manufacturer)
            .Include(x => x.CreatedBy)
            .Where(x => x.TransactionType == warehouseTransactionType)
            .Skip((pagination.Page - 1) * pagination.PerPage)
            .Take(pagination.PerPage)
            .ToListAsync();

        var totalWarehouseTransactionQuantity = await _context.WarehouseTransactions
          .Include(x => x.Manufacturer)
          .Where(x => x.TransactionType == warehouseTransactionType)
          .CountAsync();

        response.Data = _mapper.Map<IEnumerable<GetWarehouseTransactionWithoutItemDto>>(dbWarehouseTransactions);

        response.Pagination = PaginationHelper.CreatePagination(pagination, totalWarehouseTransactionQuantity);

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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_ITEM_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetWarehouseTransactionDto>> GetWarehouseTransactionAsync(int warehouseTransactionId)
    {
      var response = new ServiceResponse<GetWarehouseTransactionDto>();

      try
      {
        var dbWarehouseTransaction = await _context.WarehouseTransactions
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Images)
            .Include(x => x.Manufacturer)
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == warehouseTransactionId);

        if (dbWarehouseTransaction == null)
        {
          throw new WarehouseTransactionNotFoundException();
        }

        response.Data = _mapper.Map<GetWarehouseTransactionDto>(dbWarehouseTransaction);

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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetWarehouseTransactionDto>> UpdateWarehouseTransactionStatusAsync(int warehouseTransactionId, WarehouseTransactionStatus newStatus)
    {
      var response = new ServiceResponse<GetWarehouseTransactionDto>();

      try
      {
        var dbWarehouseTransaction = await _context.WarehouseTransactions
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Images)
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == warehouseTransactionId);

        if (dbWarehouseTransaction == null)
        {
          throw new WarehouseTransactionNotFoundException();
        }

        if (!dbWarehouseTransaction.CanModify())
        {
          throw new WarehouseTransactionModifiedException();
        }

        dbWarehouseTransaction.Status = newStatus;

        if (newStatus == WarehouseTransactionStatus.Done)
        {
          // Change the quantity of item
          var dbWarehouseTransactionItems = dbWarehouseTransaction.Items;
          var itemIds = dbWarehouseTransactionItems.Select(item => item.ProductId).ToList();

          var dbWarehouseItems = await _context.WarehouseItems
              .Where(x => itemIds.Contains(x.ProductId))
              .ToListAsync();

          foreach (var dbWarehouseItem in dbWarehouseItems)
          {
            var found = dbWarehouseTransactionItems.FirstOrDefault(tItem => tItem.ProductId == dbWarehouseItem.ProductId);
            dbWarehouseItem.Quantity += dbWarehouseTransaction.TransactionType == WarehouseTransactionType.Import
                ? found.Quantity
                : -found.Quantity;

            _context.WarehouseItems.Update(dbWarehouseItem);
          }
        }

        _context.WarehouseTransactions.Update(dbWarehouseTransaction);
        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetWarehouseTransactionDto>(dbWarehouseTransaction);

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
        response.Code = ErrorCode.WAREHOUSE_TRANSACTION_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
