namespace Product.dto
{
    public class PickPropertyDto
    {
        public string? PickId { get; set; }

        public string? CommodityId { get; set; }

        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public Dictionary<string, string> Property { get; set; } = null!;
        public decimal? Stock { get; set; }

        public IFormFile? Image { get; set; }

        public PickPropertyDto(string? pickId, string?commodityId,bool? isDeleted, decimal? price,Dictionary<string,string>property,decimal?stock,IFormFile?image)
        {
            PickId = pickId;
            CommodityId = commodityId;
            IsDeleted = isDeleted;
            Price = price;
            Property = property;
            Stock = stock;
            Image = image;
        }
    }
}
