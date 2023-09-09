using EntityFramework.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Logistics.core.dto;
using Logistics.core.vo;
using Logistics.service;
using Logistics.service.impl;

namespace Logistics.web.controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LogisticsController : ControllerBase
    {
        public static LogisticsService logisticsService;
        public LogisticsController(LogisticsService _logisticsService)
        {
            logisticsService = _logisticsService;
        }

        [HttpPost]
        public async Task<ComResponse<LogisticsInfoVo>> addLogisticsInfo([FromBody] LogisticsInfoDto logisticsinfo)
        {
            var x = await logisticsService.addLogisticsInfo(logisticsinfo.LogisticsId, logisticsinfo.ArrivePlace);
            return ComResponse<LogisticsInfoVo>.success(new LogisticsInfoVo(x));
        }

       

        [HttpPost]
        public ComResponse<IList<LogisticsInfoVo>> getAllLogisticsInfo([FromBody]LogisticIdDto id)
        {
            var x = logisticsService.getAllLogisticsInfo(id.LogisticsId);
            IList<LogisticsInfoVo> ans = new List<LogisticsInfoVo>();
            for (int i = 0; i < x.Count; i++)
                ans.Add(new LogisticsInfoVo(x[i]));
            return ComResponse<IList<LogisticsInfoVo>>.success(ans);
        }

        [HttpPost]
        public ComResponse<bool> arriveDestination([FromBody]LogisticIdDto id)
        {
            return ComResponse<bool>.success(logisticsService.arriveDestination(id.LogisticsId));
            
        }

        [HttpPost]
        public async Task<ComResponse<LogisticVo>> addLogistics([FromBody] LogisticDto logistic)
        {
            var x = await logisticsService.addLogistics(logistic.Company, logistic.ShipAddress, logistic.PickAddress);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }


        //[HttpPost]
        //public async Task<ComResponse<LogisticVo>> deleteLogistics([FromBody] string id)
        //{
        //    var x = await logisticsService.deleteLogistics(id);
        //    return ComResponse<LogisticVo>.success(new LogisticVo(x));
        //}

        [HttpPost]
        public ComResponse<LogisticVo> getLogistics([FromBody]LogisticIdDto id)
        {
            var x = logisticsService.getLogistics(id.LogisticsId);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<LogisticVo>> addArrivalTime([FromBody] LogisticIdDto id)
        {
            var x = await logisticsService.addArrivalTime(id.LogisticsId);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }

        [HttpPost]
        public ComResponse<double> setTimespan([FromBody] ClearTimeDto hours)
        {
            logisticsService.setTimespan(hours.hours);
            return ComResponse<double>.success(hours.hours);
        }



        [HttpPost]
        public ComResponse<string> openClearState()
        {  
            logisticsService.openRegularClear();
            return ComResponse<string>.success("成功开启自动清理");
        }

        [HttpPost]
        public ComResponse<string> closeClearState()
        {
            logisticsService.endRegularClear();
            return ComResponse<string>.success("成功关闭自动清理");
        }




    }
}
