using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using WebBanHang.Data;
using WebBanHang.Models;
using WebBanHang.Extensions.DataContext;
using WebBanHang.DTOs.Manufacturers;
using WebBanHang.Services.Exceptions;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.Manufacturers
{
  public class ManufacturerService : IManufacturerService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ManufacturerService> _logger;

    public ManufacturerService(DataContext context, IMapper mapper, ILogger<ManufacturerService> logger)
    {
      _context = context;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<ServiceResponse<GetManufacturerDto>> CreateManufacturerAsync(CreateManufacturerDto dto)
    {
      var response = new ServiceResponse<GetManufacturerDto>();

      try
      {
        Manufacturer newManufacturer = _mapper.Map<Manufacturer>(dto);
        await _context.Manufacturers.AddAsync(newManufacturer);
        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetManufacturerDto>(newManufacturer);
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
        response.Code = ErrorCode.MANUFACTURER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<int>> DeleteManufacturerAsync(int manufacturerId)
    {
      var response = new ServiceResponse<int>();

      try
      {
        var dbManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == manufacturerId);

        if (dbManufacturer == null)
        {
          throw new ManufacturerNotFoundException();
        }

        dbManufacturer.IsDeleted = true;
        _context.Manufacturers.Update(dbManufacturer);
        await _context.SaveChangeWithValidationAsync();

        response.Data = dbManufacturer.Id;
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
        response.Code = ErrorCode.MANUFACTURER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<List<GetManufacturerDto>>> GetAllManufacturerAsync(PaginationParam pagination, QueryManufacturerDto query)
    {
      var response = new ServiceResponse<List<GetManufacturerDto>>();
      try
      {
        var dbManufacturers = await _context.Manufacturers
          .Where(x => x.Name.Contains(query.Name))
          .Skip(pagination.Skip())
          .Take(pagination.PerPage)
          .ToListAsync();

        var totalItemsQuantity = await _context.Manufacturers
          .Where(x => x.Name.Contains(query.Name))
          .CountAsync();

        response.Data = _mapper.Map<List<GetManufacturerDto>>(dbManufacturers);
        response.Pagination = new Pagination
        {
          CurrentPage = pagination.Page,
          TotalPage = pagination.TotalPage(totalItemsQuantity),
          Count = totalItemsQuantity,
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
        response.Code = ErrorCode.MANUFACTURER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetManufacturerDto>> GetManufacturerAsync(int manufacturerId)
    {
      var response = new ServiceResponse<GetManufacturerDto>();

      try
      {
        var dbManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == manufacturerId);

        if (dbManufacturer == null)
        {
          throw new ManufacturerNotFoundException();
        }

        response.Data = _mapper.Map<GetManufacturerDto>(dbManufacturer);
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
        response.Code = ErrorCode.MANUFACTURER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetManufacturerDto>> UpdateManufacturerAsync(UpdateManufacturerDto dto)
    {
      var response = new ServiceResponse<GetManufacturerDto>();

      try
      {
        var dbManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (dbManufacturer == null)
        {
          throw new ManufacturerNotFoundException();
        }

        dbManufacturer.Name = dto.Name;
        dbManufacturer.Description = dto.Description;
        dbManufacturer.Phone = dto.Phone;
        dbManufacturer.Email = dto.Email;
        dbManufacturer.Address = dto.Address;
        dbManufacturer.Ward = dto.Ward;
        dbManufacturer.Province = dto.Province;
        dbManufacturer.Representative = dto.Representative;

        _context.Manufacturers.Update(dbManufacturer);
        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetManufacturerDto>(dbManufacturer);
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
        response.Code = ErrorCode.MANUFACTURER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
