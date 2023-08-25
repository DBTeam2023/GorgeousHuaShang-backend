namespace Product.resource.vo
{
    public class PickAuxVo
    {
        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public decimal Stock { get; set; }

        public Dictionary<string, string> Property { get; set; } = null!;

        public PickAuxVo(decimal? price,string? description,decimal stock, Dictionary<string, string> property)
        {
            Price = price;
            Description = description;
            Stock = stock;
            Property = property;
        }

    }
}
