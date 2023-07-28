#define TEST

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserIdentification.utils;
using UserIdentification.service.impl;
using UserIdentification.service;
using UserIdentification.mapper;

#if TEST

List<int> testList = new List<int>();
testList.Add(1);
testList.Add(2);
testList.Add(3);

IPage<int> testPage = IPage<int>.builder()
                                .records(testList)
                                .size(3)
                                .total(3)
                                .current(0)
                                .build();



#else

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

builder.Services.AddAuthentication(options =>
{
options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, //�Ƿ���֤Issuer
            ValidIssuer = configuration["Jwt:Issuer"], //������Issuer
            ValidateAudience = true, //�Ƿ���֤Audience
            ValidAudience = configuration["Jwt:Audience"], //������Audience
            ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
            ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
            ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
            RequireExpirationTime = true,
    };
});

builder.Services.AddSingleton(new JwtHelper(configuration));
builder.Services.AddSingleton(new ModelContext());
builder.Services.AddScoped<LoginService, LoginServiceImpl>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseCors("myCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();

#endif