using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Order.utils;
using Order.domain.model.repository;
using Order.domain.model.repository.impl;
using EntityFramework.Context;
using Order.domain.service;
using Order.domain.service.impl;
using Order.application;
using Order.application.impl;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddScoped<ModelContext>();

//repositories
builder.Services.AddScoped<OrderRepository, OrderRepositoryImpl>();

//domain services
builder.Services.AddScoped<OrderService, OrderServiceImpl>();
//application services
builder.Services.AddScoped<OrderApplicationService, OrderApplicationServiceImpl>();

//hosted services
//builder.Services.AddHostedService<StockReleaseMQListener>();
//builder.Services.AddHostedService<StockReduceMQListener>();
//builder.Services.AddHostedService<OrderDelayMQListener>();
//builder.Services.AddHostedService<OrderReleaseMQListener>();

builder.Services.AddControllers();
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

app.Run();

