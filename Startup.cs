using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

using WebBanHang.Data;
using WebBanHang.Extensions.SwashBuckle;
using WebBanHang.Services.Authorization;
using WebBanHang.Services.Users;
using WebBanHang.Filters;
using Amazon.S3;
using WebBanHang.Services.FileUploader;
using WebBanHang.Services.Categories;
using WebBanHang.Services.Products;
using WebBanHang.Models;
using WebBanHang.Services.WarehouseTransaction;
using WebBanHang.Services.WarehouseTransactionItem;
using WebBanHang.Services.WarehouseItem;
using WebBanHang.Services.Manufacturers;
using WebBanHang.Services.Baskets;
using WebBanHang.Services.Customers;
using WebBanHang.Services.BasketItems;

namespace WebBanHang
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IBasketsService, BasketsService>();
      services.AddScoped<IBasketItemsService, BasketItemsService>();
      services.AddScoped<IAuthorizationService<User>, AuthorizationService<User>>();
      services.AddScoped<IAuthorizationService<Customer>, AuthorizationService<Customer>>();
      services.AddScoped<IFileUploaderService, FileUploaderService>();
      services.AddScoped<ICustomersService, CustomersService>();
      services.AddScoped<ICategoriesService, CategoriesService>();
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IWarehouseTransactionService, WarehouseTransactionService>();
      services.AddScoped<IWarehouseTransactionItemService, WarehouseTransactionItemService>();
      services.AddScoped<IWarehouseItemService, WarehouseItemService>();
      services.AddScoped<IManufacturerService, ManufacturerService>();

      services.AddCors(options => options.AddPolicy("EasePolicy", builder =>
      {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Token-Expired");
      }));

      services.AddControllers()
          .ConfigureApiBehaviorOptions(opt =>
          {
            opt.InvalidModelStateResponseFactory = ValidateModelState.InvalidModelState;
          });

      services.AddDbContext<DataContext>(optionBuilder =>
          optionBuilder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

      services.AddAutoMapper(typeof(Startup));

      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("WebBanHang", new OpenApiInfo
        {
          Version = "v1",
          Title = "Api đồ án website bán hàng",
          Description = "Api document của đồ án website bán hàng, happy coding!",
          Contact = new OpenApiContact
          {
            Name = "Hieu the developer",
            Email = "donghuuhieu1520@gmail.com",
          }
        });

        var xmlFilePath = Path.Combine(AppContext.BaseDirectory,
                  $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

        options.IncludeXmlComments(xmlFilePath);

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
        });

        options.AddRequiredAuthorizationHeader();
      });

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(Configuration["AppSettings:SecretKey"])),
              ValidateIssuer = false,
              ValidateAudience = false,
            };

            options.Events = new JwtBearerEvents
            {
              OnAuthenticationFailed = context =>
                    {
                      if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                      {
                        context.Response.Headers.Add("Token-Expired", "true");
                      }
                      return Task.CompletedTask;
                    }
            };
          });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseCors("EasePolicy");
      app.UseSwagger();
      app.UseSwaggerUI(setupAction =>
      {
        setupAction.SwaggerEndpoint("/swagger/WebBanHang/swagger.json", "WebBanHang");
      });

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
