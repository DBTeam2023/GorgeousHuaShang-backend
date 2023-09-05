namespace UserIdentification.domain.message
{
    public class UserRegistrationMessage : MyMessage
    {
        public string userId;

        public UserRegistrationMessage(string userId)
            :base("UserRegistration")
        {
            this.userId = userId;
        }
    }
}
