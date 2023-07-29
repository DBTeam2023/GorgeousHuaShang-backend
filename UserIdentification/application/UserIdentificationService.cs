using UserIdentification.dataaccess.DBModels;
using UserIdentification.domain.model;
using UserIdentification.dto;

namespace UserIdentification.application
{
    public interface UserIdentificationService
    {
        public TokenDto login(string username, string password);

        public TokenDto register(string username, string password, string type);

        public UserAggregate getUserInfoByUsername(string username);
    }
}
