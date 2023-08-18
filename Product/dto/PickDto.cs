using System.Text.Json.Serialization;

namespace Product.dto
{
    public class PickDto
    {
        public string commodityId { get; set; } = null!;
        public Dictionary<string, string> Filter { get; set; } = null!;
        public Dictionary<string, string?> Change { get; set; } = null!;
       
    }
}
