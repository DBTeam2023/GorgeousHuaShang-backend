using Product.domain.model;

namespace Product.resource.vo
{
    public class PickVo
    {
        public string CommodityId { get; set; } = null!;

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public Dictionary<string, string> Property { get; set; } = null!;
       

        public PickVo(List<DPick> pick)
        {
            Property = new Dictionary<string, string>();
            foreach (var it in pick)
                Property.Add(it.PropertyType, it.PropertyValue);
            CommodityId = pick[0].CommodityId;
            Price = pick[0].Price;
            Description = pick[0].Description;
        }

        public static List<PickVo> createPickVo(List<IGrouping<string,DPick>> pickGroup)
        {
            var result = new List<PickVo>();
            foreach (var it in pickGroup)
                result.Add(new PickVo(it.ToList()));
            return result;
        }

    }
}
