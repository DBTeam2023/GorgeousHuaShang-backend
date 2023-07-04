using UserIdentification.entity;

namespace UserIdentification.service.impl
{
    public class LoginServiceImpl : LoginService
    {
        UserDB userDB = new UserDB();

        public bool Login(string username, string password)
        {
            return userDB.login(username, password);
        }

        public bool registerUser(string username, string password)
        {
            return userDB.registerUser(username,password);
        }
    }
}
