using EntityFramework.Models;

namespace UserIdentification.service
{
    public interface LoginService
    {
        public string Login(string username, string password);

        public string registerUser(string username, string password);

        public User getUserInfo(string token);
    }
}
