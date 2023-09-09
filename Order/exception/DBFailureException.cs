namespace Order.exception
{
    public class DBFailureException:MyException
    {
        public DBFailureException(string message) : base(message) { }
    }
}
