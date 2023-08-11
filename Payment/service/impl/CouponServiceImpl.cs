using EntityFramework.Context;
using EntityFramework.Models;
using Payment.exception;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Payment.message;
using Payment.utils;

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
        public async Task<Coupontemp> GenerateCoupon(string storeId, string commodityId, string type, decimal discount, int bar, int reduction, DateTime start, DateTime end)
        {
            if (commodityId != null)
            {
                var commodity = _context.CommodityGenerals.Where(x => x.StoreId == storeId && x.CommodityId == commodityId).FirstOrDefault();
                if (commodity == null)
                    throw new NotFoundException("Cannot find such commodity");
            }
            else
            {
                var store = _context.Stores.Where(x => x.StoreId == storeId).FirstOrDefault();
                if (store == null)
                    throw new NotFoundException("Cannot find such store");
            }

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
                CommodityId = commodityId,
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

            var coupontemplate = _context.Coupontemps.Where(x => x.CouponId == couponId).FirstOrDefault();
            if (coupontemplate == null)
                throw new NotFoundException("This coupon does not exist");

            var coupon = _context.Coupons.Where( x => x.CouponId == couponId ).FirstOrDefault();
            if (coupon!= null)
                throw new DuplicateException("This coupon has been posssessed");

            var x = new Coupon
            {
                UserId = userId,
                // copy information
                CouponId = coupontemplate.CouponId,
                StoreId = coupontemplate.StoreId,
                CommodityId = coupontemplate.CommodityId,
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
        public async Task DelCoupon(string couponId)
        {
            var coupon = _context.Coupons.Where(x => x.CouponId == couponId).FirstOrDefault();
            if (coupon == null)
                throw new NotFoundException("This coupon does not exist");

            var coupontemp = _context.Coupontemps.Where(x => x.CouponId == couponId).FirstOrDefault();

            _context.Coupons.Remove(coupon);
            _context.Coupontemps.Remove(coupontemp);
            await _context.SaveChangesAsync();
        }

        // @summary: precision query
        // @param: string couponId
        // @return: a coupon
        public async Task<Coupon> getInfo(string couponId)
        {
            //string ip = "47.115.231.142";
            string ip = "localhost";
            string user = "admin";
            string pw = "123";
            //var eventSender = new RabbitMQEventSender(ip, "my_queue", user, pw);
            //var eventData = new { EventName = "MyEvent", Data = "Event data" };
            //eventSender.SendEvent(eventData);
            var messageReceiver = new RabbitMQMessageReceiver("localhost", "my_queue");
            messageReceiver.StartReceiving();
            Console.WriteLine(messageReceiver);


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
        public async Task<IPage<Coupon>> getPage(int PageNo, int pageSize, string? userId, string? storeId, string? commodityId, string? storeName, string? commodityName)
        {
            List<Coupon> coupons = await _context.Coupons.ToListAsync();
            List<Coupon> result = new List<Coupon>();

            // get all related coupons
            if(userId != null)
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
             

            if(commodityId == null)
            {
                if (commodityName != null)
                {
                    List<Coupon> temp = new List<Coupon>();
                    var commodity = _context.CommodityGenerals.Where(x => x.CommodityName.Contains(commodityName)).ToList();
                    foreach(var com in commodity)
                    {
                        var x = result.FirstOrDefault(x => x.CommodityId == com.CommodityId);
                        if(x != null)
                            temp.Add(x);
                    }
                    result = temp;
                }
            }
            else
                result = result.Where(x => x.CommodityId == commodityId).ToList();

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
    }
}
