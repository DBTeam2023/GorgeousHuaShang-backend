namespace Payment.exception
{
    public class NotFoundException : MyException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
