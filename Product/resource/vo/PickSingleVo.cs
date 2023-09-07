using Microsoft.AspNetCore.Mvc;
using Product.domain.service;
using Product.domain.service.impl;

namespace Product.resource.vo
{
    public class PickSingleVo
    {
        public string PickId { get; set; } = null!;

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public decimal Stock { get; set; }

        public FileContentResult? Image { get; set; }

        public bool? IsDeleted { get; set; }

        public Dictionary<string, string> Property { get; set; } = null!;

        public AvatarService _avatarService = new AvatarServiceImpl();

        public PickSingleVo(string pickid,decimal? price,string? description,decimal stock, Dictionary<string, string> property,string commodityId,bool? isdeleted)
        {
            PickId = pickid;
            Price = price;
            Description = description;
            Stock = stock;
            Property = property;
            IsDeleted = isdeleted;
            Image = _avatarService.getPickAvatar(PickId, commodityId);
        }

    }
}
