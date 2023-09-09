using Product.utils;

namespace Product.dto
{
    public class CreateCommodityDto
    {
        public string StoreId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }


        public Dictionary<string, List<string>>? Property { get; set; } = null!;


        public string? ClassficationType { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public IFormFile? image { get; set; }
        public CreateCommodityDto(CreateCommodityAuxDto commodity)
        {
            Property = JsonConvertService<Dictionary<string, List<string>>>.convertToJson(commodity.Property);
            StoreId = commodity.StoreId;
            ProductName = commodity.ProductName;
            Description = commodity.Description;
            Price = commodity.Price;
            ClassficationType = commodity.ClassficationType;
            IsDeleted = commodity.IsDeleted;
            image = commodity.image;
        }
    }
}
