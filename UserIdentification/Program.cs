//#define TEST

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserIdentification.utils;
using UserIdentification.domain.model.repository;
using UserIdentification.domain.model.repository.impl;
using UserIdentification.application;
using UserIdentification.application.impl;
using UserIdentification.domain.service;
using UserIdentification.domain.service.impl;
using UserIdentification.dataaccess.mapper;
using UserIdentification.domain.model;

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

ModelContext context = new ModelContext();
UserRepository userRepository = new UserRepositoryImpl(context);
UserAggregate test = userRepository.getByUsername("buyer01");
test.buyerInfo.Address = "test address0";
userRepository.update(test);

BuyerEntity buyer = test.buyerInfo;

Type derivedType = typeof(UserRepositoryImpl);
derivedType = derivedType.BaseType;

int i = 0;

#else

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

/* Add services to the container. */
//utils
builder.Services.AddSingleton(new JwtHelper(configuration));
builder.Services.AddSingleton(new ModelContext());

//application services
builder.Services.AddScoped<UserIdentificationService, UserIdentificationServiceImpl>();

//domain services
builder.Services.AddScoped<LoginService, LoginServiceImpl>();

//repositories
builder.Services.AddSingleton<UserRepository, UserRepositoryImpl>();


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