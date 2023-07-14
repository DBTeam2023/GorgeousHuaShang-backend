using UserIdentification.mapper;

namespace UserIdentification.service.impl
{
    public class AuthorityServiceImpl
    {
        ModelContext modelContext;

        public AuthorityServiceImpl(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }
    }
}
