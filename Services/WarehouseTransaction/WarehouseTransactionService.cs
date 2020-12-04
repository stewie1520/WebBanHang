using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.Models;
using WebBanHang.Services.Exceptions;

namespace WebBanHang.Services.WarehouseTransaction
{
    public class WarehouseTransactionService : IWarehouseTransactionService
    {
        private readonly ILogger<WarehouseTransactionService> _logger;
        private readonly IMapper _mapper;
        private readonly DbContext _context;

        public WarehouseTransactionService(ILogger<WarehouseTransactionService> logger, IMapper mapper, DbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetWarehouseTransactionDto>> CreateWarehouseTransaction(CreateWarehouseTransactionDto dto)
        {
            var response = new ServiceResponse<GetWarehouseTransactionDto>();

            try
            {
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
