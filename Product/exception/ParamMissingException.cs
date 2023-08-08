namespace Product.exception
{
    public class ParamMissingException : MyException
    {
        public ParamMissingException(string message) : base(message) { }
    }
}
