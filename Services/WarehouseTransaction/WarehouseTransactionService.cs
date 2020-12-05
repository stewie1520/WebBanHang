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

namespace WebBanHang.Services.WarehouseTransaction
{
    using WebBanHang.DTOs.Commons;
    using WebBanHang.Models;

    public class WarehouseTransactionService : IWarehouseTransactionService
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

                var newTransaction = new WarehouseTransaction()
                {
                    TransactionType = dto.TransactionType,
                    CreatedAt = dto.CreatedAt,
                    Status = WarehouseTransactionStatus.Processing, // new warehouse transaction has processing status as default
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

                if (dbWarehouseTransaction.Status != WarehouseTransactionStatus.Processing)
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

        public async Task<ServiceResponse<IEnumerable<GetWarehouseTransactionWithoutItemDto>>> GetAllWarehouseTransactionsAsync(PaginationParam pagination, int type = 0)
        {
            var response = new ServiceResponse<IEnumerable<GetWarehouseTransactionWithoutItemDto>>();

            try
            {
                var warehouseTransactionType = (WarehouseTransactionType)type;

                var dbWarehouseTransactions = await _context.WarehouseTransactions
                    .Where(x => x.TransactionType == warehouseTransactionType)
                    .Skip((pagination.Page - 1) * pagination.PerPage)
                    .Take(pagination.PerPage)
                    .ToListAsync();

                response.Data = dbWarehouseTransactions
                    .Select(dbWarehouseTransaction => _mapper.Map<GetWarehouseTransactionWithoutItemDto>(dbWarehouseTransaction))
                    .ToList();

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
    }
}
