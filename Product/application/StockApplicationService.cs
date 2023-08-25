using Product.dto;

namespace Product.application
{
    public interface StockApplicationService
    {
        public Task reduceStock(ReduceStockDto reduceStock);

    }
}
