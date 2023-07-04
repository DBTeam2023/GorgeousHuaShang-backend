namespace UserIdentification.service
{
    public interface LoginService
    {
        public bool Login(string username, string password);

        public bool registerUser(string username, string password);
    }
}
