
namespace Product.domain.model
{
    public class SelectEntity
    {
        public string CommodityId { get; set; } = null!;

        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string PickId { get; set; } = null!;

        public string PropertyType { get; set; } = null!;

        public string PropertyValue { get; set; } = null!;

        public int Stock { get; set; } = 0;

        public int Count { get; set; } = 1;

        public decimal TotalAmount
        {
            get { return Price * Count ?? 0; }
        }

    }

}
