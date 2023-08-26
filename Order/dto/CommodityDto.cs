namespace Product.dto
{
    public class CommodityDto
    {
        public string StoreId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }


        public Dictionary<string, List<string>> Property { get; set; }=null!;

        public string? ClassficationType { get; set; }

        public bool? IsDeleted { get; set; } = false;

    }
}
