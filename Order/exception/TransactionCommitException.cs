namespace Order.exception
{
    public class TransactionCommitException:MyException
    {
        public TransactionCommitException(string message) : base(message) { }
    }
}
