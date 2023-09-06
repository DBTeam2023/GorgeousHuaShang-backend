using Product.domain.model;

namespace Product.dto
{
    public class CartItemDto
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;

        public Dictionary<string, string>? Filter { get; set; }
        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string PickId { get; set; } = null!;

        public Dictionary<string, List<string>> Property { get; set; } = null!;

        public List<IGrouping<string,DPick>> PickProperties { get; set; }

        public decimal Stock { get; set; }

        public decimal Count { get; set; } = 1;

        public decimal TotalAmount
        {
            get { return Price * Count ?? 0; }
        }
        public IFormFile? image { get; set; }


        public CartItemDto(string productId,string productName,bool? isDeleted,decimal? price,string?description, string pickId,Dictionary<string,List<string>> property, List<IGrouping<string, DPick>> pickProperties,decimal stock,decimal count)
        {
            ProductId = productId;
            ProductName = productName;
            IsDeleted = isDeleted;
            Price = price;
            Description = description;
            PickId = pickId;
            Property = property;
            PickProperties = pickProperties;
            Stock = stock;
            Count = count;
   



        }

    }
}
