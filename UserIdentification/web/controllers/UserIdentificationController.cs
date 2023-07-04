using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using UserIdentification.service;
using UserIdentification.service.impl;

namespace UserIdentification.web.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserIdentificationController : ControllerBase
    {
        public static LoginService loginService = new LoginServiceImpl();

        [HttpGet(Name = "login")]
        public ComResponse<bool> login(string username,string password)
        {
            Console.WriteLine(loginService.Login(username, password));
            return ComResponse<bool>.success(loginService.Login(username, password));
        }

        [HttpPost(Name = "register")]
        public ComResponse<bool> register(string username, string password)
        {
            return ComResponse<bool>.success(loginService.registerUser(username, password));
        }
    }
}
