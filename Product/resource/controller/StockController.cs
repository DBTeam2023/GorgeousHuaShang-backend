using Microsoft.AspNetCore.Mvc;
using Product.application;
using Product.dto;
using Product.utils;

namespace Product.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StockController
    {
        public StockApplicationService stockApplicationService;
        public StockController(StockApplicationService _stockApplicationService)
        {
            stockApplicationService = _stockApplicationService;
        }

        //Authorization:buyer
        [HttpPost]
        public async Task<ComResponse<string>> reduceStock(ReduceStockDto reduceStock)
        {
            await stockApplicationService.reduceStock(reduceStock);
            return ComResponse<string>.success("");
        }



    }
}
