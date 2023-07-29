using UserIdentification.application;
using UserIdentification.dataaccess.mapper;

namespace UserIdentification.application.impl
{
    public class AuthorizationServiceImpl : AuthorizationService
    {
        ModelContext modelContext;

        public AuthorizationServiceImpl(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }

        public void authorize(string id, string userType)
        {

        }
    }
}
