using System.Text.Json.Serialization;

namespace Product.domain.model
{
    public class ProductAggregate
    {
        public string StoreId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public Price Price { get; set; } = null!;

        public CommentEntity Comment { get; set; }

        public CategoryAggregate Category { get; set; }

        internal ProductAggregate() { }
        internal ProductAggregate(string storeId, string productId, Price price)
        {
            StoreId = storeId;
            ProductId = productId;
            Price = price;
        }
        internal ProductAggregate(string storeId, string productId, CommentEntity comment)
        {
            StoreId = storeId;
            ProductId = productId;
            Comment = comment;
        }
        internal ProductAggregate(string storeId, string productId, CategoryAggregate category)
        {
            StoreId = storeId;
            ProductId = productId;
            Category = category;
        }

        [JsonConstructor]
        internal ProductAggregate(string storeId, string productId, Price price, CommentEntity comment, CategoryAggregate category)
        {
            StoreId = storeId;
            ProductId = productId;
            Price = price;
            Comment = comment;
            Category = category;
        }
        //public static ProductAggregate create()
        //{

        //}


    }
}
