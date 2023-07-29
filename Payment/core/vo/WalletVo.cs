using EntityFramework.Models;
namespace Payment.core.vo
{
    // 后端给前端传递的wallet信息
    public class WalletVo
    {
        public string UserId { get; set; } = null!;

        public decimal Balance { get; set; }

        public bool Status { get; set; }

        public WalletVo(Wallet x)
        {
            UserId = x.UserId;
            Balance = x.Balance;
            Status = x.Status;
        }

    }
}
