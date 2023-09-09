using System.Collections.Generic; //·ºÐÍ¼¯ºÏ
using Product.common;
using Product.domain.model.repository;
using Product.domain.model.repository.impl;
using Product.dto;
using System.Text.Json.Serialization;
namespace Product.domain.model
{
    public class CartAggregate
    {
        public decimal Total_amount { get; set; }
        public int Total_quantity { get; set; }
        public List<PickEntity> Picks { get; set; }
        public SelectEntity? Select_items { get; set; }

        public string UserId { get; set; } = null!;
        internal CartAggregate() { }

        [JsonConstructor]
        internal CartAggregate(string userId ,List<PickEntity>picks, SelectEntity? select_items, decimal total_amount = 0, int total_quantity = 0)
        {
            UserId = userId;
            Total_amount = total_amount;
            Total_quantity = total_quantity;
            Select_items = select_items;
            Picks = picks;
        }



    }
}
