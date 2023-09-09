namespace Product.dto
{
    public class CreateCommodityAuxDto
    {
        public string StoreId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Property { get; set; } = null!;

        public string? ClassficationType { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public IFormFile? image { get; set; }

        


    }

}
