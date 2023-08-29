namespace Product.exception
{
    public class DBFailureException:MyException
    {
        public DBFailureException(string message) : base(message) { }
    }
}
