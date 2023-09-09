using EntityFramework.Context;
using EntityFramework.Models;
using Payment.exception;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Payment.message;
using Payment.utils;
using Newtonsoft.Json.Linq;

namespace Payment.service.impl
{
    public class CouponServiceImpl: CouponService
    {
        private readonly ModelContext _context;
        private static System.Timers.Timer? _timer;

        public CouponServiceImpl(ModelContext context)
        {
            _context = context;
            _timer = new System.Timers.Timer();
            _timer.Interval = 60 * 60 * 1000;  // an hour
            _timer.Elapsed += async (s, e) => await TimedEvent();
            _timer.Start();
        }

        // @summary: generate a random string
        // @param: int length
        // @return: a string 
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // @summary: generate a coupon
        // @param: coupon info
        // @return: a generated coupon 
        public async Task<Coupontemp> GenerateCoupon(string storeId, string type, decimal discount, int bar, int reduction, DateTime start, DateTime end)
        {
            // generate coupon_id randomly ,and check if there is repeatment
            while (true)
            {
                string randomString = GenerateRandomString(10);
                if (_context.Coupons.Where(x => x.CouponId == randomString).FirstOrDefault() == null)
                    break;
            }

           var coupontemplate = new Coupontemp
            {
                CouponId = GenerateRandomString(10),
                StoreId = storeId,  
                Type = type,
                Discount = discount,   
                Bar = bar,
                Reduction = reduction,
                Validfrom = start,
                Validto = end,
            };
            await _context.Coupontemps.AddAsync(coupontemplate);
            await _context.SaveChangesAsync();
            return coupontemplate;
        }

        // @summary: add a coupon to a user
        // @param: string userId, string couponId
        // @return: the coupon added to a user
        public async Task<Coupon> AddCoupon(string userId, string couponId)
        {
            var user = _context.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user == null)
                throw new NotFoundException("This user does not exist");
            if (user.Type == "seller")
                throw new NotFoundException("This user is not a buyer.");

            var coupontemplate = _context.Coupontemps.Where(x => x.CouponId == couponId).FirstOrDefault();
            if (coupontemplate == null)
                throw new NotFoundException("This coupon does not exist");

            var coupon = _context.Coupons.Where( x => x.CouponId == couponId && x.UserId == userId).FirstOrDefault();
            if (coupon!= null)
                throw new DuplicateException("This coupon has been owned by this user.");

            var x = new Coupon
            {
                UserId = userId,
                // copy information
                CouponId = coupontemplate.CouponId,
                StoreId = coupontemplate.StoreId,
                Type = coupontemplate.Type,
                Discount = coupontemplate.Discount,
                Bar = coupontemplate.Bar,
                Reduction = coupontemplate.Reduction,
                Validfrom = coupontemplate.Validfrom,
                Validto = coupontemplate.Validto,
            };
            await _context.Coupons.AddAsync(x);
            await _context.SaveChangesAsync();
            return x;
        }

        // @summary: delete a coupon both in the Coupons and the Coupontemps
        // @param: string couponId
        // @return: void
        public async Task DelCouponByBuyer(string couponId, string token)
        {
            string userId = await getUserId(token);
            var coupon = _context.Coupons.Where(x => x.CouponId == couponId && x.UserId == userId).FirstOrDefault();
            if (coupon == null)
                throw new NotFoundException("This coupon does not exist");

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task DelCouponByStore(string couponId)
        {
            var coupontemp = _context.Coupontemps.Where(x => x.CouponId == couponId).FirstOrDefault();
            if (coupontemp == null)
                throw new NotFoundException("This coupon does not exist");

            var coupon = _context.Coupons.Where(x => x.CouponId == couponId).ToList();

            _context.Coupontemps.Remove(coupontemp);
            _context.Coupons.RemoveRange(coupon);
            await _context.SaveChangesAsync();
        }

        // @summary: precision query
        // @param: string couponId
        // @return: a coupon
        public async Task<Coupon> getInfo(string couponId)
        {
            var coupon = await _context.Coupons.Where(_x => _x.CouponId == couponId).FirstOrDefaultAsync();
            if (coupon == null)
                throw new NotFoundException("This coupon does not exist");
          
            return coupon;
        }

        // @summary: page query
        // @param:
        // current         current page
        // size            the number of records in a page
        // userId, storeId, commodityId
        // @return: all coupons that satisfy the conditions
        public async Task<IPage<Coupon>> getPage(int PageNo, int pageSize, string? userId, string? storeId,  string? storeName)
        {
            List<Coupon> coupons = await _context.Coupons.ToListAsync();
            List<Coupon> result = new List<Coupon>();
            // get all related coupons
            if (userId != null)
                result.AddRange(coupons.Where(x => x.UserId == userId).ToList());
            else
                result.AddRange(coupons);

            if (storeId == null)
            {
                if(storeName != null)
                {
                    List<Coupon> temp = new List<Coupon>();
                    var store = _context.Stores.Where(x => x.StoreName.Contains(storeName)).ToList();
                    foreach (var s in store)
                    {
                        var x = result.FirstOrDefault(x => x.StoreId == s.StoreId);
                        if (x != null)
                            temp.Add(x);
                    }
                    result = temp;
                }
            }
            else
                result = result.Where(x => x.StoreId == storeId).ToList();
             
            // return the certain list
            var list = result.Skip((PageNo - 1) * pageSize)
               .Take(pageSize)
               .ToList();
            IPage<Coupon> Page = IPage<Coupon>.builder()
               .records(list)
               .total(result.Count)
               .size(pageSize)
               .current(PageNo)
               .build();
            return Page;
        }


        public async Task<IPage<Coupontemp>> getStoreCoupon(int pageNo, int pageSize, string storeId)
            {
            List<Coupontemp> result = new List<Coupontemp>();
            result = _context.Coupontemps.Where(x => x.StoreId == storeId).ToList();

            var list = result.Skip((pageNo - 1) * pageSize)
              .Take(pageSize)
              .ToList();
            IPage<Coupontemp> Page = IPage<Coupontemp>.builder()
               .records(list)
               .total(result.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;
        }

        public async Task<IPage<Coupon>> getValid(int pageNo, int pageSize, string token, List<string> pickIds)
                {
            var userId = await getUserId(token);
            Dictionary<string, decimal?> storePicks = new Dictionary<string, decimal?>();
            List<Coupon> result = new List<Coupon>();
            // 获取店铺与消费金额的键值对
            foreach(var pickId in pickIds)
                    {
                var x = _context.Picks.FirstOrDefault(x => x.PickId == pickId);
                if (x == null)
                    throw new NotFoundException("This pick of commodity does not exist.");
                var commodity = _context.CommodityGenerals.FirstOrDefault(s => s.CommodityId == x.CommodityId);
                if(commodity == null)
                    throw new NotFoundException("This commodity does not exist.");
                if(storePicks.ContainsKey(commodity.StoreId))
                {
                    if (x.Price == null)
                        storePicks[commodity.StoreId] += commodity.Price;
                    else
                        storePicks[commodity.StoreId] += x.Price;
                    }
                else
                {
                    if (x.Price == null)
                        storePicks.Add(commodity.StoreId, commodity.Price);
                    else
                        storePicks.Add(commodity.StoreId, x.Price);
                }
            }

            // 选取可以使用的优惠券
            foreach(var kvp in storePicks)
            {
                string store = kvp.Key;
                decimal? price = kvp.Value;
                var coupons = _context.Coupons.Where(x => x.StoreId == store && x.UserId == userId).ToList();
                foreach(var coupon in coupons)
                {
                    if(coupon.Type == "discount")
                        result.Add(coupon);
            else
                    {
                        if (coupon.Bar < price)
                            result.Add(coupon);
                    }
                }
            }

            var list = result.Skip((pageNo - 1) * pageSize)
               .Take(pageSize)
               .ToList();
            IPage<Coupon> Page = IPage<Coupon>.builder()
               .records(list)
               .total(result.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;
        }

        public async Task<decimal?> calculate(List<string> pickIds, string couponId, string token)
        {
            // 获取钱包信息
            string userId = await getUserId(token);
            var wallet = _context.Wallets.FirstOrDefault(s => s.UserId == userId);
            if (wallet == null)
                throw new NotFoundException("This wallet does not exist.");
            if (wallet.Status == false)
                throw new StatusException("The wallet is frozen.");

            // 计算总金额
            Dictionary<string, decimal?> storePicks = new Dictionary<string, decimal?>();
            foreach (var pickId in pickIds)
            {
                var x = _context.Picks.FirstOrDefault(x => x.PickId == pickId);
                if (x == null)
                    throw new NotFoundException("This pick of commodity does not exist.");
                var commodity = _context.CommodityGenerals.FirstOrDefault(s => s.CommodityId == x.CommodityId);
                if (commodity == null)
                    throw new NotFoundException("This commodity does not exist.");
                if (storePicks.ContainsKey(commodity.StoreId))
                {
                    if (x.Price == null)
                        storePicks[commodity.StoreId] += commodity.Price;
                    else
                        storePicks[commodity.StoreId] += x.Price;
                }
                else
                {
                    if (x.Price == null)
                        storePicks.Add(commodity.StoreId, commodity.Price);
                    else
                        storePicks.Add(commodity.StoreId, x.Price);
                }
            }
            var coupon = _context.Coupons.FirstOrDefault(s => s.CouponId == couponId);
            if (coupon == null)
                throw new NotFoundException("This coupon does not exist.");
            if (coupon.Type == "discount")
            {
                Console.WriteLine(storePicks[coupon.StoreId]);
                storePicks[coupon.StoreId] *= coupon.Discount;
                Console.WriteLine(storePicks[coupon.StoreId]);
            }
            else
            {
                if(storePicks[coupon.StoreId] >= coupon.Bar)
                    storePicks[coupon.StoreId] -= coupon.Reduction;
            }
            decimal? result = 0;
            foreach (var kvp in storePicks)
            {
                var storeId = kvp.Key;
                result += kvp.Value;
                var seller = _context.SellerStores.Where(s => s.StoreId == storeId && s.Ismanager == 1).FirstOrDefault();
                if (seller == null)
                    throw new NotFoundException("This seller can not be found.");
                var sellerWallet = _context.Wallets.FirstOrDefault(s => s.UserId == seller.UserId);
                if (sellerWallet == null)
                    throw new NotFoundException("This seller does not have a wallet.");
                sellerWallet.Balance += (decimal)kvp.Value;
            }

            // 扣钱加钱操作
            if (result > wallet.Balance)
                throw new RangeException("Your balance is not enough.");
            wallet.Balance -= (decimal)result;
            _context.SaveChanges();

            await DelCouponByBuyer(couponId, token);
            return result;
        }

        // @summary: clean expired coupons
        // @param: void
        // @return: void
        public async Task Clean()
        {
            var expiredCoupons = _context.Coupons.Where(c => c.Validto < DateTime.Now);
            _context.Coupons.RemoveRange(expiredCoupons);
            await _context.SaveChangesAsync();
        }
        
        private async Task TimedEvent()
        {
            await Clean();
        }
       

        public async Task<string>getUserId(string token)
        {
            if (token == null)
                throw new NotFoundException("This token is null.");
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
                throw new NotFoundException("Can not find this user.");

            return userId;
        }


    }


}
