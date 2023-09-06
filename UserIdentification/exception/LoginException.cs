namespace UserIdentification.exception
{
    public class LoginException : MyException
    {
        public bool IsLoggedIn { get; set; } = false;

        public bool IsLoggedOut { get; set; } = false;

        public bool UserNotFound { get; set; } = false;

        public bool PasswordError { get; set; } = false;

        public LoginException(string message) : base(message) { }
    }
}
