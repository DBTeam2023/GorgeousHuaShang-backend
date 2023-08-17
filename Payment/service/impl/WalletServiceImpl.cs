using EntityFramework.Context;
using EntityFramework.Models;
using Payment.exception;
using Newtonsoft.Json.Linq;

namespace Payment.service.impl
{
    public class WalletServiceImpl : WalletService
    {
        private readonly ModelContext _context;

        public WalletServiceImpl(ModelContext context)
        {
            _context = context;
        }


        // @summary: get the information of a wallet by id
        // @param: string userId
        // @return: a wallet struct
        public async Task<Wallet> GetWallet(string userId)
        {
            var wallet = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();

            if (wallet == null)
                throw new NotFoundException("This user does not exist");
            return wallet;
        }


        // @summary: add a wallet by userId
        // @param: string userId
        // @return: a wallet struct
        public async Task<Wallet> AddWallet(string userId, decimal balance)
        {
            var wallet = new Wallet()
            {
                UserId = userId,
                Balance = balance,
                Status = true
            };

            if (_context.Wallets.Any(x => x.UserId == userId))
                throw new DuplicateException("This user has already had a wallet");

            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        // @summary: delete a wallet by id
        // @param: string userId
        // @return: void
        public async Task DelWallet(string userId)
        {
            var wallet = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();

            if (wallet == null)
                throw new NotFoundException("This user does not exist");

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        // @summary: recharge a wallet
        // @param: string userId, decimal amount
        // @return: a wallet struct
        public async Task<Wallet> RechargeWallet(string userId, decimal amount)
        {
            var wallet = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();

            if (wallet == null)
                throw new NotFoundException("This user does not exist");

            if (wallet.Status == false)
                throw new StatusException("The wallet is frozen");

            wallet.Balance += amount;
            await _context.SaveChangesAsync();
            return wallet;

        }

        // @summary: deduct a wallet
        // @param: string userId, decimal amount
        // @return: a struct including info of the revised wallet
        public async Task<Wallet> DeductWallet(string userId, decimal amount)
        {
            var wallet = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();
            if (wallet == null)
                throw new NotFoundException("This user does not exist");

            if (wallet.Balance < amount)
                throw new RangeException("The balance is insufficient");

            if (wallet.Status == false)
                throw new StatusException("The wallet is frozen");


            wallet.Balance -= amount;
            await _context.SaveChangesAsync();
            return wallet;
        }

        // @summary: set the status of a wallet
        // @param: string userId, bool status
        // @return: a struct including info of the revised wallet
        public async Task<Wallet> SetStatus(string userId, bool status)
        {
            var wallet = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();
            if (wallet == null)
                throw new NotFoundException("This user does not exist");

            wallet.Status = status;
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<string> getUserId(string token)
        {
            string userId;
            string url = "http://47.115.231.142:1025/UserIdentification/getUserInfo";

            HttpClient client = new HttpClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);
                HttpResponseMessage response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                JObject code = JObject.Parse(responseBody);
                Console.WriteLine(code);

                // 获取 userId 字段的值
                userId = (string)code["data"]["userId"];

                Console.WriteLine(userId);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            if (userId == null)
                throw new NotFoundException("There is no match to this token.");

            return userId;
        }

    }
}
