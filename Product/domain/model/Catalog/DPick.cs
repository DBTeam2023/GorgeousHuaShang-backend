namespace Product.domain.model
{
    public class DPick
    {
        public string CommodityId { get; set; } = null!;

        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string PickId { get; set; } = null!;

        public string PropertyType { get; set; } = null!;

        public string PropertyValue { get; set; } = null!;

        public decimal Stock { get; set; } = 0;


    }
}
