using EntityFramework.Models;

namespace UserIdentification.service
{
    public interface LoginService
    {
        public string login(string username, string password);

        public string registerUser(string username, string password,string type);

        public User getUserInfo(string token);
    }
}
