namespace Product.dto
{
    public class PickAuxDto
    {
        public string? PickId { get; set; }

        public string? commodityId { get; set; }
        public string? Filter { get; set; }
        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public decimal? Stock { get; set; }

        public IFormFile? image { get; set; }
    }
}
