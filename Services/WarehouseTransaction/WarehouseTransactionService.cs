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
    }
}
