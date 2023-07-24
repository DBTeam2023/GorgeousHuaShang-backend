using UserIdentification.exception;

namespace UserIdentification.common
{
    public class UserType
    {
        public static readonly string Administrator = "admin";
        public static readonly string Buyer = "buyer";
        public static readonly string Seller = "seller";

        public static bool TypeCheck(string type)
        {
            switch (type)
            {
                case "admin":
                case "buyer":
                case "seller":
                    return true;
                default:
                    throw new InvalidTypeException("invalid user type");
            }

            return false;
        }
    }
}
