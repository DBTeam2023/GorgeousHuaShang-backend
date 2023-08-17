namespace Payment.exception
{
    public class DuplicateException : MyException
    {
        public DuplicateException(string message) : base(message) { }
    }
}
