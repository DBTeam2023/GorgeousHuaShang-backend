//using Product.common;
//using Product.exception;

//namespace Product.domain.model
//{
//    public class Price
//    {
//        public string Currency { get; set; } = null!;

//        public double Number { get; set; }

//        public Price(string currency, double number)
//        {
//            CurrencyType.TypeCheck(currency);
//            Currency = currency;
//            Number = number;
//        }

//        public string getPriceStr()
//        {
//            CurrencyType.TypeCheck(Currency);
//            return Number.ToString() + " " + Currency;
//        }

//        public static Price convertToPrice(string str)
//        {
//            var price = str.Split();
//            if (price.Count() != 2)
//                throw new InvalidTypeException("invalid price convert type");

//            double number = 0;
//            if (!double.TryParse(price[0], out number))
//                throw new InvalidTypeException("invalid price convert type");
//            else
//                return new Price(price[1], number);


//        }
//    }
//}
