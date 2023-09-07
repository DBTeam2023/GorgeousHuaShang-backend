using Product.dto;

namespace Product.application
{
    public interface StockApplicationService
    {
        public Task reduceStock(StockDto reduceStock);

        public Task restoreStock(StockDto reduceStock);

        public Task LockStock(StockLockDto stockLockDto);

    }
}
