using Product.common;
using Product.domain.model.repository;
using Product.domain.model.repository.impl;
using Product.dto;
using System.Text.Json.Serialization;

namespace Product.domain.model
{
    public class ProductAggregate
    {
        public string StoreId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        //public CommentEntity? Comment { get; set; }

        public CategoryAggregate Category { get; set; }

        public bool? IsDeleted { get; set; } = false;

     
        internal ProductAggregate() { }
        
        [JsonConstructor]
        internal ProductAggregate( string storeId, string productId,
            string productName, decimal price,string? description,
            CategoryAggregate category, bool? isDeleted = false)
        {
            
            StoreId = storeId;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Description = description;
            IsDeleted = isDeleted;
            Category = category;
        }

        //初始化时候
        public static ProductAggregate create(CreateCommodityDto commodity)
        {
            var guid = Guid.NewGuid().ToString();

            var category = new CategoryAggregate(guid,
                new List<DPick>(), commodity.Property,commodity.ClassficationType);

            return new ProductAggregate(
                commodity.StoreId, guid, commodity.ProductName,
                commodity.Price, commodity.Description, category, commodity.IsDeleted);
        }

        //非初始化时候
        public static ProductAggregate create(CommodityDto commodity)
        {
            var category = new CategoryAggregate(commodity.ProductId,
                new List<DPick>(), commodity.Property, commodity.ClassficationType);

            return new ProductAggregate(
                commodity.StoreId, commodity.ProductId, commodity.ProductName,
                commodity.Price, commodity.Description, category, commodity.IsDeleted);
        }




    }
}
