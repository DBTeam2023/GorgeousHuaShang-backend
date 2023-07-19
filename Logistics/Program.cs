//#define TEST

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Logistics.utils;
using EntityFramework.Context;
using Logistics.service;
using Logistics.service.impl;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

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

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true, //�Ƿ���֤Issuer
//            ValidIssuer = configuration["Jwt:Issuer"], //������Issuer
//            ValidateAudience = true, //�Ƿ���֤Audience
//            ValidAudience = configuration["Jwt:Audience"], //������Audience
//            ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
//            ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
//            ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
//            RequireExpirationTime = true,
//        };
//    });

builder.Services.AddSingleton<ModelContext>();
builder.Services.AddScoped<LogisticsService,LogisticsServiceImpl>();

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





////ɾ��һ��
//public async Task<Logisticsinfo> deleteLogisticsInfo(string id, string place, DateTime time)
//{           

//    var to_be_del = _context.Logisticsinfos.Where(e => e.LogisticsId == id && e.ArrivePlace == place && e.ArriveTime == time).FirstOrDefault();
//    if (to_be_del==null)
//        throw new NotFoundException("cannot find such logisticsinfo");

//    _context.Logisticsinfos.Remove(to_be_del);

//    await _context.SaveChangesAsync();
//    return to_be_del;

//}

////ɾ������
//public async Task<IList<Logisticsinfo>> deleteAllLogisticsInfo(string id)
//{    
//    var to_be_deleted = _context.Logisticsinfos.Where(e => e.LogisticsId == id);
//    if(to_be_deleted.Count()==0)
//        throw new NotFoundException("cannot find such logisticsinfo");
//    _context.Logisticsinfos.RemoveRange(to_be_deleted);
//    await _context.SaveChangesAsync();
//    return to_be_deleted.ToList();

//}
