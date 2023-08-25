using Product.domain.service;
using Product.dto;

namespace Product.application.Stock.impl
{
    public class StockApplicationServiceImpl:StockApplicationService
    {
        private StockService _stockService;

        public StockApplicationServiceImpl(StockService stockService)
        {
            _stockService = stockService;
        }
        public async Task reduceStock(ReduceStockDto reduceStock)
        {
            await _stockService.reduceStock(reduceStock);
        }

    }
}
