using UserIdentification.mapper;

namespace UserIdentification.service.impl
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
