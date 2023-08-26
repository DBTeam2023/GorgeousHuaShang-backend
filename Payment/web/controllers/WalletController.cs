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
        public async Task<ComResponse<WalletVo>> get([FromHeader]string token)
        {
            string userId = await walletService.getUserId(token);
            var x = await walletService.GetWallet(userId);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<WalletVo>> add([FromHeader]string token, [FromBody] WalletInfoDto walletInfo)
        {
            string userId = await walletService.getUserId(token);
            var x = await walletService.AddWallet(userId, walletInfo.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpDelete]
        public async Task delete([FromHeader]string token)
        {
            string userId = await walletService.getUserId(token);
            await walletService.DelWallet(userId);
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> recharge([FromHeader]string token, [FromBody]WalletRechargeDto walletRecharge)
        {
            string userId = await walletService.getUserId(token);
            var x = await walletService.RechargeWallet(userId, walletRecharge.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> deduct([FromHeader]string token, [FromBody] WalletRechargeDto walletDeduct)
        {
            string userId = await walletService.getUserId(token);
            var x = await walletService.DeductWallet(userId, walletDeduct.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> SetStatus([FromHeader]string token, [FromBody] WalletSetDto walletSet)
        {
            string userId = await walletService.getUserId(token);
            var x = await walletService.SetStatus(userId, walletSet.status);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

    }
}
