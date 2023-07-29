using Microsoft.AspNetCore.Mvc;
using Payment.core.dto;
using Payment.core.vo;
using Payment.service;


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
        public ComResponse<WalletVo> get([FromBody] UserIdDto userIdDto)
        {
            var x = walletService.GetWallet(userIdDto.userId);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<WalletVo>> add([FromBody] WalletInfoDto walletInfo)
        {
            var x = await walletService.AddWallet(walletInfo.userId, walletInfo.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpDelete]
        public async Task delete([FromBody] UserIdDto userIdDto)
        {
            await walletService.DelWallet(userIdDto.userId);
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> recharge([FromBody] WalletRechargeDto walletRecharge)
        {
            var x = await walletService.RechargeWallet(walletRecharge.userId, walletRecharge.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> deduct([FromBody] WalletRechargeDto walletDeduct)
        {
            var x = await walletService.DeductWallet(walletDeduct.userId, walletDeduct.amount);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

        [HttpPut]
        public async Task<ComResponse<WalletVo>> SetStatus([FromBody] WalletSetDto walletSet)
        {
            var x = await walletService.SetStatus(walletSet.userId, walletSet.status);
            return ComResponse<WalletVo>.success(new WalletVo(x));
        }

    }
}
