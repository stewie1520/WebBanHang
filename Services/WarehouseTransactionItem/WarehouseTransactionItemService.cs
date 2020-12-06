using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using WebBanHang.DTOs.WarehouseTransactionItems;
using WebBanHang.Services.Exceptions;
using WebBanHang.Data;
using WebBanHang.Extensions.DataContext;

namespace WebBanHang.Services.WarehouseTransactionItem
{
    using WebBanHang.Models;

    public class WarehouseTransactionItemService : IWarehouseTransactionItemService
    {
        private readonly ILogger<WarehouseTransactionItemService> _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public WarehouseTransactionItemService(ILogger<WarehouseTransactionItemService> logger,
                                               IMapper mapper,
                                               DataContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetWarehouseTransactionItemDto>> AddItemToTransactionAsync(int warehouseTransactionId, AddWarehouseItemDto dto)
        {
            var response = new ServiceResponse<GetWarehouseTransactionItemDto>();

            try
            {
                var dbWarehouseTransaction = await _context.WarehouseTransactions
                    .FirstOrDefaultAsync(x => x.Id == warehouseTransactionId);

                if (dbWarehouseTransaction == null)
                {
                    throw new WarehouseTransactionNotFoundException();
                }

                if (!dbWarehouseTransaction.CanModify())
                {
                    throw new WarehouseTransactionModifiedException();
                }

                var dbProduct = await _context.Products
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == dto.ProductId);

                if (dbProduct == null)
                {
                    throw new ProductNotFoundException();
                }

                var existedWarehouseTransactionItem = await _context.WarehouseTransactionItems
                        .Include(x => x.WarehouseTransaction)
                        .Include(x => x.Product)
                        .ThenInclude(x => x.Category)
                        .FirstOrDefaultAsync(x => x.ProductId == dto.ProductId &&
                                                  x.WarehouseTransaction.Id == warehouseTransactionId);

                if (existedWarehouseTransactionItem != null)
                {
                    existedWarehouseTransactionItem.Quantity = dto.Quantity;

                    _context.WarehouseTransactionItems.Update(existedWarehouseTransactionItem);
                    await _context.SaveChangeWithValidationAsync();

                    response.Data = _mapper.Map<GetWarehouseTransactionItemDto>(existedWarehouseTransactionItem);
                }
                else
                {
                    var newWarehouseTransactionItem = new WarehouseTransactionItem()
                    {
                        WarehouseTransaction = dbWarehouseTransaction,
                        ProductId = dbProduct.Id,
                        Product = dbProduct,
                        Quantity = dto.Quantity,
                    };

                    await _context.WarehouseTransactionItems.AddAsync(newWarehouseTransactionItem);
                    await _context.SaveChangeWithValidationAsync();

                    response.Data = _mapper.Map<GetWarehouseTransactionItemDto>(newWarehouseTransactionItem);
                }


                return response;
            }
            catch (BaseServiceException ex)
            {
                response.Success = false;
                response.Code = ex.Code;
                response.Message = ex.ErrorMessage;


                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Code = ErrorCode.WAREHOUSE_TRANSACTION_ITEM_UNEXPECTED_ERROR;
                response.Message = ex.Message;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }

        public Task<ServiceResponse<GetWarehouseTransactionItemDto>> AddItemToTransactionAsync(int warehouseTransactionId, IEnumerable<AddWarehouseItemDto> dtos)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<RemoveWarehouseItemDto>> RemoveItemFromTransactionAsync(int warehouseTransactionId, RemoveWarehouseItemDto dto)
        {
            var response = new ServiceResponse<RemoveWarehouseItemDto>();

            try
            {
                Expression<Func<WarehouseTransactionItem, bool>> condition = x => dto.ProductId.HasValue
                    ? x.ProductId == dto.ProductId
                    : x.Id == dto.WarehouseTransactionItemId;


                var dbTransactionItem = await _context.WarehouseTransactionItems
                    .Include(x => x.WarehouseTransaction)
                    .FirstOrDefaultAsync(condition);

                if (dbTransactionItem == null)
                {
                    throw new WarehouseTransactionItemNotFoundException();
                }

                if (!dbTransactionItem.WarehouseTransaction?.CanModify() ?? true)
                {
                    throw new WarehouseTransactionModifiedException();
                }

                dbTransactionItem.IsDeleted = true;

                _context.WarehouseTransactionItems.Update(dbTransactionItem);
                await _context.SaveChangeWithValidationAsync();

                response.Data = dto;
                return response;
            }
            catch (BaseServiceException ex)
            {
                response.Success = false;
                response.Code = ex.Code;
                response.Message = ex.ErrorMessage;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Code = ErrorCode.WAREHOUSE_TRANSACTION_ITEM_UNEXPECTED_ERROR;
                response.Message = ex.Message;

                _logger.LogError(ex.Message, ex.StackTrace);
                return response;
            }
        }
    }
}
