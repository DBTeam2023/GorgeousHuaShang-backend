using EntityFramework.Models;
namespace Payment.service
{
    public interface WalletService
    {
        // operations on wallet list
        public Wallet GetWallet(string userId); 
        public Task<Wallet> AddWallet(string userId, decimal balance);  
        public Task DelWallet(string userId); 

        // operations on single wallet
        public Task<Wallet> RechargeWallet(string userId, decimal amount);
        public Task<Wallet> DeductWallet(string userId, decimal amount);
        public Task<Wallet> SetStatus(string userId, bool status);
    }

}
