using EntityFramework.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using UserIdentification.core.dto;
using UserIdentification.core.vo;
using UserIdentification.service;
using UserIdentification.service.impl;

/**
 * @author sty
 * 
 */
namespace UserIdentification.web.controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserIdentificationController : ControllerBase
    {
        public static LoginService loginService;

        public UserIdentificationController(LoginService _loginService)
        {
            loginService = _loginService;
        }

        [HttpPost]
        public ComResponse<TokenDto> login([FromBody] UserDto user)
        {
            return ComResponse<TokenDto>.success(new TokenDto(loginService.Login(user.Username, user.Password)));
        }

        [HttpPost]
        public ComResponse<TokenDto> register([FromBody] UserDto user)
        {
            return ComResponse<TokenDto>.success(new TokenDto(loginService.registerUser(user.Username, user.Password)));
        }

        [HttpGet]
        public async Task<ComResponse<UserInfoVo>> getUserInfo()
        {
            string token = Request.Headers["Authorization"];
            var newToken = token.Replace("Bearer ", "");
            return ComResponse<UserInfoVo>.success(new UserInfoVo(loginService.getUserInfo(newToken)));
        }
    }
}
