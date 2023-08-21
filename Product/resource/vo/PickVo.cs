using Product.domain.model;

namespace Product.resource.vo
{
    public class PickVo
    {
        public string CommodityId { get; set; } = null!;

        public List<PickAuxVo> CommodityInfo { get;set; }
       

        private void addCommodityInfo(List<DPick> pick)
        {
            CommodityId = pick[0].CommodityId;
            var property = new Dictionary<string, string>();
            foreach (var it in pick)
                property.Add(it.PropertyType, it.PropertyValue);
            CommodityInfo.Add(new PickAuxVo(pick[0].Price, pick[0].Description,pick[0].Stock, property));
        }


        public PickVo(List<IGrouping<string, DPick>> pickGroup)
        {
            CommodityInfo = new List<PickAuxVo>();
            foreach (var it in pickGroup)
                this.addCommodityInfo(it.ToList());
                          
         
        }

      

    }
}
