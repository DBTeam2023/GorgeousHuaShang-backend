using Microsoft.AspNetCore.Mvc;
using UserIdentification.entity;
using UserIdentification.utils;

namespace UserIdentification.service.impl
{
    public class LoginServiceImpl : LoginService
    {
        static UserDB userDB = new UserDB();
        JwtHelper jwt;

        public LoginServiceImpl(JwtHelper _jwt)
        {
            jwt = _jwt;
        }

        public string Login(string username, string password)
        {
            //@TODO: exception handling
            if(userDB.login(username, password) == false)
            {
                return "test";
            }

            return jwt.CreateToken(username);
        }

        public string registerUser(string username, string password)
        {
            userDB.registerUser(username, password);
            return jwt.CreateToken(username);
        }
    }
}
