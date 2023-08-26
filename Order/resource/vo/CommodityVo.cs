﻿using Product.domain.model;
using Product.utils;

namespace Product.resource.vo
{
    public class CommodityVo
    {
        public string StoreId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public CommodityVo(ProductAggregate productAggregate)
        {
            StoreId = productAggregate.StoreId;
            ProductId = productAggregate.ProductId;
            ProductName = productAggregate.ProductName;
            Description = productAggregate.Description;
            Price = productAggregate.Price;
        }

        public static IPage<CommodityVo> createCommodityPageVo(IPage<ProductAggregate> productAggrgatePage)
        {
            var commodityVoList = new List<CommodityVo>();
            foreach (var it in productAggrgatePage.Records)
                commodityVoList.Add(new CommodityVo(it));

            return IPage<CommodityVo>.builder().records(commodityVoList)
                .size(productAggrgatePage.Size).total(productAggrgatePage.Total)
                .current(productAggrgatePage.Current).build();
        }


    }
}
