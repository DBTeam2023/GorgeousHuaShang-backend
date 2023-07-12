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
        public ComResponse<string> login([FromBody] UserDto user)
        {
            return ComResponse<string>.success(loginService.Login(user.Username, user.Password));
        }

        [HttpPost("newuser")]
        public ComResponse<string> register([FromBody] UserDto user)
        {
            return ComResponse<string>.success(loginService.registerUser(user.Username, user.Password));
        }

        [HttpGet("user")]
        public ComResponse<UserInfoVo> getUserInfo([FromBody] string token)
        {
            return ComResponse<UserInfoVo>.success(new UserInfoVo(loginService.getUserInfo(token)));
        }
    }
}
