﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebBanHang.Data;
using WebBanHang.DTOs.User;
using WebBanHang.Models;
using WebBanHang.Services.Exceptions;
using WebBanHang.Extensions.DataContext;

namespace WebBanHang.Services.Users
{
  public partial class UserService : BaseService, IUserService
  {
    private readonly DataContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(DataContext context, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _logger = logger;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<GetUserDto>> CreateUser(CreateUserDto newUser)
    {
      var response = new ServiceResponse<GetUserDto>();
      try
      {
        var user = _mapper.Map<User>(newUser);
        var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (userExist != null)
        {
          response.Success = false;
          response.Message = "User has already existed";
          return response;
        }
        (user.Password, user.PasswordSalt) = CreateHashedPassword(newUser.Password);

        foreach (int roleId in newUser.Roles)
        {
          var existRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
          if (existRole != null)
          {
            var role = new UserRole { Role = existRole, User = user };
            await _context.UserRoles.AddAsync(role);
          }
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        response.Data = _mapper.Map<GetUserDto>(user);

      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
        return response;
      }
      return response;
    }

    public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers(int page, int perpage)
    {
      var response = new ServiceResponse<List<GetUserDto>>();
      try
      {
        var skip = (page - 1) * perpage;
        var userInDb = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .Skip(skip)
            .Take(perpage)
            .ToListAsync();
        var TotalPage = await _context.Users.CountAsync();
        response.Data = userInDb.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
        response.Pagination = new Pagination
        {
          CurrentPage = page,
          TotalPage = (int)Math.Ceiling(1.0m * TotalPage / perpage)
        };

      }
      catch (Exception ex)
      {
        response.Success = false;
        response.Message = ex.Message;
        return response;
      }
      return response;
    }

    public async Task<ServiceResponse<GetUserDto>> GetUserAsync(string userEmail)
    {
      var response = new ServiceResponse<GetUserDto>();

      try
      {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
        if (dbUser == null)
        {
          throw new UserNotFoundException();
        }

        response.Data = _mapper.Map<GetUserDto>(dbUser);
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
        response.Code = ErrorCode.USER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }

    public async Task<ServiceResponse<GetUserDto>> UpdateUserAsync(string userEmail, UpdateUserDto dto)
    {
      var response = new ServiceResponse<GetUserDto>();
      try
      {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

        if (dbUser == null)
        {
          throw new UserNotFoundException();
        }

        dbUser.Avatar = dto.Avatar;
        dbUser.BirthDate = dto.BirthDate;
        dbUser.Gender = dto.Gender;
        dbUser.Address = dto.Address;
        dbUser.Name = dto.Name;
        dbUser.Phone = dto.Phone;

        _context.Users.Update(dbUser);
        await _context.SaveChangeWithValidationAsync();

        response.Data = _mapper.Map<GetUserDto>(dbUser);

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
        response.Code = ErrorCode.USER_UNEXPECTED_ERROR;

        _logger.LogError(ex.Message, ex.StackTrace);
        return response;
      }
    }
  }
}
