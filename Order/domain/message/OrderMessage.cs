namespace Order.domain.message
{
    public class OrderMessage
    {
        public string orderId { get; set; }
        public List<string> pickIds { get; set; }
    }
}
