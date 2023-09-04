using Microsoft.AspNetCore.Mvc;
using Product.domain.model;
using Product.dto;
using Product.utils;

namespace Product.application
{
    public interface ProductApplicationService
    {

        public Task<CommodityIdDto> createCommodity(CreateCommodityDto commodity);

        public ProductAggregate getCommodity(CommodityIdDto commodityId);
        public Task updateCommodity(CommodityDto commodity);

        public Task updatePick(PickDto pick);

        //public Task updatePick(List<PickDto> pick);
        public IPage<ProductAggregate> commodityPageQuery(PageQueryDto pageQuery);
        public PickGroupDto displayPicks(CommodityIdDto commodityId);

        public PickGroupDto getSinglePick(PickIdDto pickId);
        public Task deleteCommodity(CommodityIdDto commodityId);

    }
}
