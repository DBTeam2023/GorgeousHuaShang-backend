using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using Product.application;
//using Product.domain.model;
//using Product.domain.service;
//using Product.dto;
//using Product.exception;
//using Product.resource.vo;
//using Product.utils;
using Product.domain.model.repository.impl;
using Product.common;
using Product.domain.model;
using EntityFramework.Context;

/**
 * @author sty
 * @implnote use "[action]" in URL for allowing asynchronous http request
 */
namespace Product.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        //public static JwtHelper jwt;
        //public static ProductService ProductService;
        

        [HttpPost]
        public async Task add()
        {
            CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
            //var ans=cat.getById("1");
            var property = new List<KeyValuePair<string, List<string>>>();
            property.Add(new KeyValuePair<string, List<string>>("color", new List<string> { "red", "yellow" }));
            property.Add(new KeyValuePair<string, List<string>>("size", new List<string> { "big", "small" }));
            CategoryAggregate? xxx = new CategoryAggregate("1", new List<DPick>(), property, new TrousersType());
            cat.add(xxx);


        }

        //[HttpPost]
        //public ComResponse<TokenDto> register([FromBody] RegisterDto user)
        //{
        //    return ComResponse<TokenDto>.success(ProductService.register(user.Username, user.Password, user.Type));
        //}

        //[HttpPost]
        //public ComResponse<int> update([FromBody] UserAggregate userAggregate)
        //{
        //    ProductService.update(userAggregate);

        //    return ComResponse<int>.success(0);
        //}

        //[HttpGet]
        //public async Task<ComResponse<UserAggregate>> getUserInfo()
        //{
        //    //resolve token
        //    string token = Request.Headers["Authorization"].ToString();
        //    if (token == null)
        //    {
        //        throw new ParamMissingException("token required");
        //    }
        //    var newToken = token.Replace("Bearer ", "");
        //    var username = jwt.resolveToken(newToken);

        //    return ComResponse<UserAggregate>.success(ProductService.getUserInfoByUsername(username));
        //}
    }
}
