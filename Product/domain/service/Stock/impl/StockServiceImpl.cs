using EntityFramework.Context;
using Product.domain.model.repository;
using Product.dto;
using Product.exception;

namespace Product.domain.service.Stock.impl
{
    public class StockServiceImpl:StockService
    {
        private readonly ModelContext modelContext;
        public CategoryRepository categoryRepository;
        public StockServiceImpl(ModelContext _modelContext, CategoryRepository _categoryRepository)
        {
            modelContext = _modelContext;
            categoryRepository = _categoryRepository;
        }


       

        public async Task reduceStock(ReduceStockDto reduceStock)
        {
            var pickdto = new PickDto
            {
                commodityId = reduceStock.CommodityId,
                Filter = reduceStock.Filter
            };

            decimal original_stocks = 0;
            var first_pick = categoryRepository.getPicks(pickdto);

            if (first_pick.Count() == 0)
                throw new NotFoundException("no picks found");
            
           
            original_stocks = first_pick[0].First().Stock;
            

            if(reduceStock.reduceNum<= 0)
                throw new StockException("reduce number should be larger than 0");

            if (original_stocks < reduceStock.reduceNum)
                throw new StockException("Insufficient stock");

            await categoryRepository.setPick(new PickDto()
            {
                commodityId = reduceStock.CommodityId,
                Filter = reduceStock.Filter,
                Stock = original_stocks - reduceStock.reduceNum,
            });
        }
    }
}
