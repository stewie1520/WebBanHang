using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;

using WebBanHang.Data;
using WebBanHang.DTOs.Baskets;
using WebBanHang.Models;
using WebBanHang.Extensions.DataContext;
using WebBanHang.DTOs.BasketItems;
using WebBanHang.Services.Customers;
using WebBanHang.DTOs.Customers;
using WebBanHang.DTOs.Commons;
using WebBanHang.Commons;
using WebBanHang.Services.Exceptions;

namespace WebBanHang.Services.Baskets
{
  public class BasketsService : BaseService, IBasketsService
  {
    private readonly DataContext _context;
    private readonly ILogger<BasketsService> _logger;
    private readonly IMapper _mapper;
    private readonly ICustomersService _customerService;
    private readonly IBasketItemsService _basketItemService;
    public BasketsService(DataContext context, IMapper mapper, ILogger<BasketsService> logger, ICustomersService customerService, IBasketItemsService basketItemService)
    {
      _context = context;
      _mapper = mapper;
      _logger = logger;
      _customerService = customerService;
      _basketItemService = basketItemService;
    }
    public async Task<ServiceResponse<GetBasketDto>> CreateBasketAsync(CreateBasketDto createBasketDto)
    {
      var response = new ServiceResponse<GetBasketDto>();
      try
      {
        var basket = _mapper.Map<Basket>(createBasketDto);
        // Check for customer if email exist
        if (createBasketDto.Email != null)
        {

          var customer = _context.Customers.FirstOrDefault(c => c.Email == createBasketDto.Email);
          if (customer == null)
          {
            // Tạo khách hàng (bỏ sau)
            var customerDto = new CreateCustomerDto
            {
              Email = createBasketDto.Email,
              FullName = createBasketDto.FullName,
              Gender = Gender.Unknown,
              Address = createBasketDto.Address
            };
            var createNewCustomerResult = await _customerService.CreateCustomerAsync(customerDto);
            if (!createNewCustomerResult.Success)
            {
              response.Success = false;
              response.Message = createNewCustomerResult.Message;

              return response;
            }
            customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == createNewCustomerResult.Data.Id);
          }
          basket.Customer = customer;

        }
        // Add basket item

        await _context.Baskets.AddAsync(basket);

        var getBasketDto = _mapper.Map<GetBasketDto>(basket);

        var TotalPrice = 0;

        foreach (CreateBasketItemDto createBasketItem in createBasketDto.Items)
        {
          var newBasketItem = await _basketItemService.CreateBasketItemAsync(createBasketItem, basket);

          TotalPrice += newBasketItem.Data.Price;
        }

        basket.TotalPrice = TotalPrice;
        basket.IsPaid = false;
        basket.Status = BasketStatus.Ordering;


        await _context.SaveChangeWithValidationAsync();

        response.Data = getBasketDto;

        response.Data.TotalPrice = TotalPrice;

        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.BASKET_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<IEnumerable<GetBasketWithoutItemDto>>> GetAllBasketsAsync(PaginationParam pagination, QueryBasketDto query)
    {
      var response = new ServiceResponse<IEnumerable<GetBasketWithoutItemDto>>();

      try
      {
        var dbBasketsQuery = _context.Baskets.AsQueryable();

        if (query.From.HasValue)
        {
          // GTE
          dbBasketsQuery = dbBasketsQuery.Where(basket => basket.UpdatedAt.CompareTo(query.From.Value) > -1);
        }

        if (query.To.HasValue)
        {
          // LTE
          dbBasketsQuery = dbBasketsQuery.Where(basket => basket.UpdatedAt.CompareTo(query.To.Value) < 1);
        }

        if (query.Status.HasValue)
        {
          dbBasketsQuery = dbBasketsQuery.Where(basket => basket.Status == query.Status);
        }


        var dbBaskets = await dbBasketsQuery
            .Include(x => x.Customer)
            .Skip((pagination.Page - 1) * pagination.PerPage)
            .Take(pagination.PerPage)
            .ToListAsync();
        var totalBasketQuantity = await dbBasketsQuery
            .Include(x => x.BasketItems)
            .CountAsync();

        response.Data = _mapper.Map<IEnumerable<GetBasketWithoutItemDto>>(dbBaskets);

        response.Pagination = PaginationHelper.CreatePagination(pagination, totalBasketQuantity);

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
        response.Code = ErrorCode.BASKET_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
    public async Task<ServiceResponse<GetBasketDto>> GetBasketAsync(int basketId)
    {
      var response = new ServiceResponse<GetBasketDto>();

      try
      {
        var dbBasket = await _context.Baskets
            .Include(x => x.BasketItems)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(c => c.Id == basketId);

        if (dbBasket == null)
        {
          throw new BasketNotFoundException();
        }

        response.Data = _mapper.Map<GetBasketDto>(dbBasket);

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
        response.Code = ErrorCode.BASKET_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetBasketDto>> UpdateBasketStatusAsync(UpdateBasketStatusDto updateBasketStatusDto)
    {
      var response = new ServiceResponse<GetBasketDto>();
      try
      {
        var basket = await _context.Baskets.Include(x => x.Customer)
                                            .FirstOrDefaultAsync(c => c.Id == updateBasketStatusDto.Id);

        if (basket == null)
        {
          response.Success = false;
          response.Message = "Basket is not found";
          response.Code = ErrorCode.BASKET_NOT_FOUND_ERROR;

          return response;
        }
        basket.Status = updateBasketStatusDto.Status;

        _context.Baskets.Update(basket);

        await _context.SaveChangeWithValidationAsync();

        var getBasketDto = _mapper.Map<GetBasketDto>(basket);

        response.Data = getBasketDto;

        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
        response.Code = ErrorCode.BASKET_NOT_FOUND_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
