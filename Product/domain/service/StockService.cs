using Product.dto;

namespace Product.domain.service
{
    public interface StockService
    {
        public Task restoreStock(StockDto restoreStock);
        public Task reduceStock(StockDto reduceStock);
    }
}
