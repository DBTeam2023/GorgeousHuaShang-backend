using EntityFramework.Context;
using Product.domain.model;
using Product.domain.model.repository;
using Product.dto;
using Product.exception;
using Product.utils;

namespace Product.domain.service.impl
{
    public class ProductServiceImpl:ProductService
    {
        private readonly ModelContext modelContext;
        public ProductRepository productRepository;
        public CategoryRepository categoryRepository;
        public ProductServiceImpl(ModelContext _modelContext, ProductRepository _productRepository, CategoryRepository _categoryRepository)
        {
            modelContext = _modelContext;
            productRepository = _productRepository;
            categoryRepository = _categoryRepository;

        }


        //for buyer
        public List<IGrouping<string,DPick>> displayPicks(CommodityIdDto commodityId)
        {
            var productAggregate=productRepository.getById(commodityId.commodityId);

            //已经下架了
            if(productAggregate.IsDeleted==true)
                return new List<IGrouping<string, DPick>>();
            

            var price = productAggregate.Price;
            var description = productAggregate.Description;
            productAggregate.Category.DetailPicks = productAggregate.Category.DetailPicks
                .Where(p => p.IsDeleted == false).ToList();

            foreach (var it in productAggregate.Category.DetailPicks)
            {
                if (it.Price == null)
                    it.Price = price;
                if (it.Description == null)
                    it.Description = description;
            }

           
            return productAggregate.Category.DetailPicks.GroupBy(g => g.PickId).ToList();

           
        }

        
        //for buyer
        public List<IGrouping<string, DPick>> getPick(PickIdDto pickId)
        {
           
            var pick = categoryRepository.getPicks(new PickDto(new PickAuxDto
            {
                PickId = pickId.PickId,
            }));

            if (pick.Count() == 0)
                throw new NotFoundException("pick not found");

            var commodityId = pick[0].ElementAt(0).CommodityId;


            var productAggregate = productRepository.getById(commodityId);

            var price = productAggregate.Price;
            var description = productAggregate.Description;

            foreach (var it in pick)
            {
                foreach(var p in it)
                {
                    if (p.Price == null)
                        p.Price = price;
                    if (p.Description == null)
                        p.Description = description;
                }
                
            }

            return pick;


        }





    }
}
