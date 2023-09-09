using Microsoft.AspNetCore.Mvc;
using Payment.core.dto;
using Payment.core.vo;
using Payment.service;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Payment.web.controllers
{
    [ApiController]
    [Route("Payment/[controller]/[action]")]
    public class WalletController: ControllerBase
    {
        public static WalletService walletService;

        public WalletController(WalletService _walletService)
        {
            walletService = _walletService;
        }

        [HttpPost]
        public async Task<ComResponse<WalletVo>> get()
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            var x = await walletService.GetWallet(userId);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<WalletVo>> add([FromBody] WalletInfoDto walletInfo)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            var x = await walletService.AddWallet(userId, walletInfo.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpDelete]
        public async Task delete()
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            await walletService.DelWallet(userId);
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> recharge([FromBody]WalletRechargeDto walletRecharge)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            var x = await walletService.RechargeWallet(userId, walletRecharge.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> deduct([FromBody] WalletRechargeDto walletDeduct)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            var x = await walletService.DeductWallet(userId, walletDeduct.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> SetStatus([FromBody] WalletSetDto walletSet)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await walletService.getUserId(token);
            var x = await walletService.SetStatus(userId, walletSet.status);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

    }
}
