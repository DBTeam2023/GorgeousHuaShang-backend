﻿using Product.domain.model;
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
        public Task updatePick(List<PickDto> pick);
        public IPage<ProductAggregate> commodityPageQuery(PageQueryDto pageQuery);
        public List<IGrouping<string,DPick>> displayPicks(CommodityIdDto commodityId);
        public Task deleteCommodity(CommodityIdDto commodityId);

    }
}