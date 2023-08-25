namespace Product.dto
{
    public class ReduceStockDto
    {
        public string CommodityId { get; set; } = null!;
        public Dictionary<string, string> Filter { get; set; } = null!;

        public decimal reduceNum { get; set; }


    }
}
