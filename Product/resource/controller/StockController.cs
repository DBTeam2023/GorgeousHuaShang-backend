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
        public async Task<ComResponse<string>> reduceStock([FromBody]StockDto reduceStock)
        {
            await stockApplicationService.reduceStock(reduceStock);
            return ComResponse<string>.success("reduce stock");
        }

        //Authorization:buyer
        [HttpPost]
        public async Task<ComResponse<string>> restoreStock([FromBody] StockDto restoreStock)
        {
            await stockApplicationService.restoreStock(restoreStock);
            return ComResponse<string>.success("restore stock");
        }



    }
}
