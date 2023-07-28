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
            ValidateIssuer = true, //是否验证Issuer
            ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
            ValidateAudience = true, //是否验证Audience
            ValidAudience = configuration["Jwt:Audience"], //订阅人Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
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