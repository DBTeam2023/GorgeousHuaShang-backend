using Microsoft.AspNetCore.Mvc;
using Order.domain.service;
using Order.domain.service.impl;

namespace Order.resource.vo
{
    public class PickSingleVo
    {
        public string PickId { get; set; } = null!;

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public decimal Stock { get; set; }

        public FileContentResult? Image { get; set; }

        public Dictionary<string, string> Property { get; set; } = null!;


        public PickSingleVo(string pickid, decimal? price, string? description, decimal stock, Dictionary<string, string> property, string commodityId)
        {
            PickId = pickid;
            Price = price;
            Description = description;
            Stock = stock;
            Property = property;
        }

    }
}
