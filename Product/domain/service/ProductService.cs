using Product.domain.model;
using Product.dto;

namespace Product.domain.service
{
    public interface ProductService
    {
        public List<IGrouping<string,DPick>> displayPicks(CommodityIdDto commodityId);

        public List<IGrouping<string, DPick>> getPick(PickIdDto pickId);





    }
}
