namespace Product.exception
{
    public class TransactionCommitException:MyException
    {
        public TransactionCommitException(string message) : base(message) { }
    }
}
