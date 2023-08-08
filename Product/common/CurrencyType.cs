using Product.exception;

namespace Product.common
{
    public class CurrencyType
    {
        public static readonly string Dollar = "dollar";
        public static readonly string Pound = "pound";
        public static readonly string RMB = "rmb";

        public static bool TypeCheck(string type)
        {
            switch (type)
            {
                case "dollar":
                case "pound":
                case "rmb":
                    return true;
                default:
                    throw new InvalidTypeException("invalid currency type");
            }

            
        }
    }
}
