namespace UserIdentification.entity
{
    public class UserDB
    {
        List<User> users = new List<User>();

        public UserDB() { }

        public bool registerUser(string username,string password)
        {
            User newUser = new User(username,password,0,"0");
            users.Add(newUser);
            return true;
        }

        public bool login(string username,string password)
        {
            User targetUser = users.Find(x => x.Username == username);
            if (targetUser != null)
            {
                if (targetUser.Password == password)
                    return true;
            }

            return false;
        }
    }
}
