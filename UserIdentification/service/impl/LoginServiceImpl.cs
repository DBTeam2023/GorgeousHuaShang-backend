using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using UserIdentification.exception;
using UserIdentification.mapper;
using UserIdentification.utils;

namespace UserIdentification.service.impl
{
    public class LoginServiceImpl : LoginService
    {
        JwtHelper jwt;
        ModelContext modelContext;

        public LoginServiceImpl(JwtHelper _jwt, ModelContext dbcontext)
        {
            jwt = _jwt;
            modelContext = dbcontext;
        }

        public string Login(string username, string password)
        {
            // @TODO: exception handling

            //var loginUser = modelContext.Find<User>(username);
            var loginUser = modelContext.Users.Where(x  => x.NickName == username).FirstOrDefault();
            if(loginUser==null)
            {
                return "test";
            }

            return jwt.CreateToken(username);
        }

        public string registerUser(string username, string password)
        {
            // TODO: exception handling
            var existUser = modelContext.Users.Where(x => x.NickName == username).FirstOrDefault();
            if(existUser!=null)
            {
                throw new DuplicateException("username already exists");
            }

            Guid id = Guid.NewGuid();
            var newUser = new User()
            {
                UserId = id.ToString(),
                NickName = username,
                Password = password,
                Type = "buyer"
            };
            modelContext.Add(newUser);
            modelContext.SaveChanges();

            return jwt.CreateToken(username);
        }

        public User getUserInfo(string token)
        {
            var username = jwt.resolveToken(token);
            var user = modelContext.Users.Where(x => x.NickName == username).FirstOrDefault();

            // TODO: exception handling

            return user;
        }
    }
}
