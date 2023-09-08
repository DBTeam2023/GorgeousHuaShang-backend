using Order.exception;

namespace Order.common
{
    public class OrderStateType
    {
        public static readonly string SuccessPaid = "paid";
        public static readonly string WaitToPay = "wait to pay";
        public static readonly string CancelPay = "cancelled";
        public static readonly string CompletePay = "order complete";
        public static void TypeCheck(string state)
        {
            if (state == SuccessPaid) return;
            if (state == WaitToPay) return;
            if (state == CancelPay) return;
            if (state == CompletePay) return;
            throw new OrderStateException("no such order state");
        }

    }
}
