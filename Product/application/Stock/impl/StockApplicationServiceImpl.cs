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
        public async Task reduceStock(StockDto reduceStock)
        {

            await _stockService.reduceStock(reduceStock);
        }

        public async Task restoreStock(StockDto reduceStock)
        {


            await _stockService.restoreStock(reduceStock);
        }

        public async Task LockStock(StockEventDto stockEventDto)
        {
            await _stockService.LockStock(stockEventDto);
        }

        public async Task<bool> IsEnoughStock(StockDto stockEventDto)
        {
            return await _stockService.IsEnoughStock(stockEventDto);
        }




    }
}
