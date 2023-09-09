using Microsoft.AspNetCore.Mvc;

namespace Order.domain.model.Order
{
    public class DPick
    {
        public string CommodityId { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string PickId { get; set; } = null!;

        public Dictionary<string, string> Property { get; set; } = null!;

        //public FileContentResult? PickImage { get; set; } = null!;
        public string Image { get; set; } = null!;

        public string ImageType { get; set; } = null!;

        public decimal Number { get; set; }
    }
}
