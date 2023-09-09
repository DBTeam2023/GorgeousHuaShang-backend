using Microsoft.AspNetCore.Mvc;
using Product.common;
using Product.domain.model;
using Product.domain.service;
using Product.domain.service.impl;

namespace Product.resource.vo
{
    public class ProductInfoVo
    {
        public class ProductPickVo
        {
            public string CommodityId { get; set; } = null!;



            public string PickId { get; set; } = null!;

            public decimal? Price { get; set; }

            public string? Description { get; set; }

            public Dictionary<string, string> Property { get; set; } = null!;

            public bool? IsDeleted { get; set; }

            public decimal Stock { get; set; }

            public FileContentResult? Image { get; set; }

            public AvatarService _avatarService = new AvatarServiceImpl();


            public ProductPickVo(List<DPick> pick)
            {
                Property=new Dictionary<string, string>();
                foreach (var it in pick)
                    Property.Add(it.PropertyType, it.PropertyValue);
                CommodityId = pick[0].CommodityId;
                Price = pick[0].Price;
                Description = pick[0].Description;
                IsDeleted = pick[0].IsDeleted;
                Stock = pick[0].Stock;
                PickId = pick[0].PickId;
                Image=_avatarService.getPickAvatar(PickId, CommodityId);
            }

            public static List<ProductPickVo> createProductPickVo(List<IGrouping<string, DPick>> pickGroup)
            {
                var result = new List<ProductPickVo>();
                foreach (var it in pickGroup)
                    result.Add(new ProductPickVo(it.ToList()));
                return result;
            }

        }



        public string StoreId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public Dictionary<string, List<string>> Property { get; set; } = null;

        public string? ClassficationType { get; set; }

        public FileContentResult? Image { get; set; }

        public List<ProductPickVo> DetailPicks { get; set; }

        public AvatarService _avatarService = new AvatarServiceImpl();


        public ProductInfoVo(ProductAggregate productAggregate)
        {
            
            StoreId = productAggregate.StoreId;
            ProductId = productAggregate.ProductId;
            ProductName = productAggregate.ProductName;
            Description = productAggregate.Description;
            Price = productAggregate.Price;
            IsDeleted = productAggregate.IsDeleted;
            Property = productAggregate.Category.Property;
            ClassficationType = productAggregate.Category.ClassficationType;

            Image = _avatarService.getCommodityAvatar(ProductId);
      
            DetailPicks = ProductPickVo.createProductPickVo(
                productAggregate.Category.DetailPicks.GroupBy(g => g.PickId).ToList());
        }


    }
}
