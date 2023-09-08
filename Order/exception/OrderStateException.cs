namespace Order.exception
{
    public class OrderStateException:MyException
    {
        public OrderStateException(string message) : base(message) { }
    }
}
