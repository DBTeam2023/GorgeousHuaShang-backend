using UserIdentification.dataaccess.DBModels;
using UserIdentification.dto;

namespace UserIdentification.domain.service
{
    public interface LoginService
    {
        public TokenDto login(string username, string password);

        public TokenDto register(string username, string password, string type);

        public TokenDto resetPassword(string username, string newPassword);
    }
}
