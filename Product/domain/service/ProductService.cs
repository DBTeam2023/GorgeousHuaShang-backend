using Product.domain.model;
using Product.dto;

namespace Product.domain.service
{
    public interface ProductService
    {
        public PickGroupDto displayPicks(CommodityIdDto commodityId);

        public PickGroupDto getPick(PickIdDto pickId);





    }
}
