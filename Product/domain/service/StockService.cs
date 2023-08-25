using Product.dto;

namespace Product.domain.service
{
    public interface StockService
    {
      
        public Task reduceStock(ReduceStockDto reduceStock);
    }
}
