using UserIdentification.entity;

namespace UserIdentification
{
    public class testUserIdentification
    {
        static void Main(string[] args)
        {
            UserDB db = new UserDB();
            db.registerUser("sty", "12345");
            Console.WriteLine(db.login("sty", "12345"));
        }
    }
}
