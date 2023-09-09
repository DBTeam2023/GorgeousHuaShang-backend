namespace Product.dto
{
    public class StockLockMessage
    {
        public string pickId { get; set; }
        public string orderId { get; set; }
        public int number { get; set; }

        //TODO: init
        public bool isReduced { get; set; }
    }
}
