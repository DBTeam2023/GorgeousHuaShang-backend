using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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

        [Route("user")]
        [HttpPost]
        public ComResponse<string> login(string username,string password)
        {
            return ComResponse<string>.success(loginService.Login(username, password));
        }

        [Route("newuser")]
        [HttpPost]
        public ComResponse<string> register(string username, string password)
        {
            return ComResponse<string>.success(loginService.registerUser(username, password));
        }
    }
}
