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
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;

        public Dictionary<string, string>? Filter { get; set; }
        public bool? IsDeleted { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string PickId { get; set; } = null!;

        public Dictionary<string, List<string>> Property { get; set; } = null!;

        public List<IGrouping<string, DPick>> PickProperties { get; set; }

        public decimal Stock { get; set; }

        public decimal Count { get; set; } = 1;

        public decimal TotalAmount
        {
            get { return Price * Count ?? 0; }
        }
        public FileContentResult? Image { get; set; }

        public AvatarService _avatarService = new AvatarServiceImpl();

        public CartItemSingleVo(CartItemDto cartItem)
        {
            ProductId = cartItem.ProductId;
            ProductName = cartItem.ProductName;
            IsDeleted = cartItem.IsDeleted;
            Price = cartItem.Price;
            Description = cartItem.Description;
            PickId = cartItem.PickId;
            Property = cartItem.Property;
            PickProperties = cartItem.PickProperties;
            Stock = cartItem.Stock;
            Count = cartItem.Count;
            Image = _avatarService.getPickAvatar(cartItem.PickId,cartItem.ProductId);
        }
    }
}
