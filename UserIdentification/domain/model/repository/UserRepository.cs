namespace UserIdentification.domain.model.repository
{
    public interface UserRepository
    {
        public void add(UserAggregate userAggregate);

        public void update(UserAggregate userAggregate);

        public void delete(string userId);

        public UserAggregate getById(string userId);

        public UserAggregate getByUsername(string username);
    }
}
