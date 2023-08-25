using System.Text.Json.Serialization;

namespace Product.dto
{
    public class PickDto
    {
        public string commodityId { get; set; } = null!;
        public Dictionary<string, string> Filter { get; set; } = null!;
        //Change
        public bool? IsDeleted { get; set; }
        
        public decimal? Price { get; set; }
        
        public string? Description { get; set; }

        public decimal? Stock { get; set; }
    }
}
