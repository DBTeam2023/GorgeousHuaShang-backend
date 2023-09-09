using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Product.utils;
using Product.domain.model.repository;
using Product.domain.model.repository.impl;
using EntityFramework.Context;
using Product.domain.service;
using Product.domain.service.impl;
using Product.application;
using Product.application.impl;
using Product.domain.service.Stock.impl;
using Product.application.Stock.impl;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddScoped<ModelContext>();

//repositories
builder.Services.AddScoped<CategoryRepository, CategoryRepositoryImpl>();
builder.Services.AddScoped<ProductRepository, ProductRepositoryImpl>();
builder.Services.AddScoped<CartRepository, CartRepositoryImpl>();

//domain services
builder.Services.AddScoped<ProductService, ProductServiceImpl>();
builder.Services.AddScoped<StockService, StockServiceImpl>();
builder.Services.AddScoped<AvatarService, AvatarServiceImpl>();
builder.Services.AddScoped<CartService, CartServiceImpl>();

//application services
builder.Services.AddScoped<ProductApplicationService, ProductApplicationServiceImpl>();
builder.Services.AddScoped<StockApplicationService, StockApplicationServiceImpl>();
builder.Services.AddScoped<CartApplicationService, CartApplicationServiceImpl>();
builder.Services.AddControllers();

//hosted services
builder.Services.AddHostedService<StockReleaseMQListener>();
builder.Services.AddHostedService<StockReduceMQListener>();
builder.Services.AddHostedService<StockDelayMQListener>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy
      (name: "myCors",
        builde =>
        {
            builde.WithOrigins("*", "*", "*")
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
        }
      );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

await app.RunAsync(); // 使用 await 关键字异步运行应用程序

