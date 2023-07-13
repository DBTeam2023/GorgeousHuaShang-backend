using EntityFramework.Models;
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
    [Route("[controller]")]
    public class UserIdentificationController : ControllerBase
    {
        public static LoginService loginService;

        public UserIdentificationController(LoginService _loginService)
        {
            loginService = _loginService;
        }

        [HttpPost("user")]
        public ComResponse<TokenDto> login([FromBody] UserDto user)
        {
            return ComResponse<TokenDto>.success(new TokenDto(loginService.Login(user.Username, user.Password)));
        }

        [HttpPost("newuser")]
        public ComResponse<TokenDto> register([FromBody] UserDto user)
        {
            return ComResponse<TokenDto>.success(new TokenDto(loginService.registerUser(user.Username, user.Password)));
        }

        [HttpPost("userInfo")]
        public ComResponse<UserInfoVo> getUserInfo([FromBody] TokenDto token)
        {
            return ComResponse<UserInfoVo>.success(new UserInfoVo(loginService.getUserInfo(token.Token)));
        }
    }
}
