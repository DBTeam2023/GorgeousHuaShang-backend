namespace Order.exception
{
    public class StockException:MyException
    {
        public StockException(string message) : base(message) { }
    }
}
