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
            var x = await logisticsService.addLogisticsInfo(logisticsinfo.LogisticsId, logisticsinfo.ArrivePlace, logisticsinfo.ArriveTime);
            return ComResponse<LogisticsInfoVo>.success(new LogisticsInfoVo(x));
        }

        //[HttpPost]
        //public async Task<ComResponse<LogisticsInfoVo>> deleteLogisticsInfo([FromBody] LogisticsInfoDto logisticsinfo)
        //{
        //    var x = await logisticsService.deleteLogisticsInfo(logisticsinfo.LogisticsId, logisticsinfo.ArrivePlace, logisticsinfo.ArriveTime);
        //    return ComResponse<LogisticsInfoVo>.success(new LogisticsInfoVo(x));
        //}

        //[HttpPost]
        //public async Task<ComResponse<IList<LogisticsInfoVo>>> deleteAllLogisticsInfo([FromBody] string id)
        //{
        //    var x = await logisticsService.deleteAllLogisticsInfo(id);
        //    IList<LogisticsInfoVo> ans_del = new List<LogisticsInfoVo>();
        //    for (int i = 0; i < x.Count; i++)
        //        ans_del.Add(new LogisticsInfoVo(x[i]));

        //    return ComResponse<IList<LogisticsInfoVo>>.success(ans_del);
        //}

        [HttpPost]
        public ComResponse<IList<LogisticsInfoVo>> getAllLogisticsInfo([FromBody] string id)
        {
            var x = logisticsService.getAllLogisticsInfo(id);
            IList<LogisticsInfoVo> ans = new List<LogisticsInfoVo>();
            for (int i = 0; i < x.Count; i++)
                ans.Add(new LogisticsInfoVo(x[i]));
            return ComResponse<IList<LogisticsInfoVo>>.success(ans);
        }

        [HttpGet]
        public bool arriveDestination(string id)
        {
            return logisticsService.arriveDestination(id);
        }

        [HttpPost]
        public async Task<ComResponse<LogisticVo>> addLogistics([FromBody] LogisticDto logistic)
        {
            var x = await logisticsService.addLogistics(logistic.StartTime, logistic.Company, logistic.ShipAddress, logistic.PickAddress);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }


        //[HttpPost]
        //public async Task<ComResponse<LogisticVo>> deleteLogistics([FromBody] string id)
        //{
        //    var x = await logisticsService.deleteLogistics(id);
        //    return ComResponse<LogisticVo>.success(new LogisticVo(x));
        //}

        [HttpPost]
        public ComResponse<LogisticVo> getLogistics([FromBody] string id)
        {
            var x = logisticsService.getLogistics(id);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<LogisticVo>> addArrivalTime(LogisticEndDto logistic_end)
        {
            var x = await logisticsService.addArrivalTime(logistic_end.LogisticsId, logistic_end.EndTime);
            return ComResponse<LogisticVo>.success(new LogisticVo(x));
        }

        [HttpPost]
        public ComResponse<double> setTimespan(double hours)
        {
            logisticsService.setTimespan(hours);
            return ComResponse<double>.success(hours);
        }



        [HttpPost]
        public async Task openClearState()
        {  
            logisticsService.openRegularClear();      
        }

        [HttpPost]
        public void closeClearState()
        {
            logisticsService.endRegularClear();
        }




    }
}
