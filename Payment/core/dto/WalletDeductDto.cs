namespace Payment.core.dto
{
    // 用于扣除钱包的数据传输组件
    public class WalletDeductDto
    {
        public string userId { get; set; }
        public decimal amount { get; set; }
    }
}
