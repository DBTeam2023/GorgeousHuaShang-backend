using Microsoft.AspNetCore.Mvc;
using Product.domain.model;
using Product.domain.service.impl;
using Product.domain.service;
using Product.dto;
using EntityFramework.Models;

namespace Product.resource.vo
{
    public class CartItemSingleVo
    {
        public PickVo Pick { get; set; } = null!;

        public decimal Count { get; set; } = 1;

        public decimal TotalAmount
        {
            get { return Pick.CommodityInfo[0].Price * Count ?? 0; }
        }

        public CartItemSingleVo(PickVo pick, decimal count)
        {
            Pick = pick;
            Count = count;
        }
    }
}
