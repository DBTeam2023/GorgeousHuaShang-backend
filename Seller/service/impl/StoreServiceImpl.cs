using EntityFramework.Context;
using EntityFramework.Models;
using Storesys.exception;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Storesys.utils;
using Newtonsoft.Json.Linq;

namespace Storesys.service.impl
{
    public class StoreServiceImpl: StoreService
    {
        private readonly ModelContext _context;

        public StoreServiceImpl(ModelContext context)
        {
            _context = context;
        }
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<Store> add(string token, string storeName, int isManager)
        {
            string userId = await getUserId(token);
            var seller = _context.Sellers.FirstOrDefault(s => s.UserId == userId);
            if (seller == null)
                throw new NotFoundException("This user is not a seller.");
            var store = new Store
            {
                StoreId = GenerateRandomString(10),
                StoreName = storeName,
                Score = 0,
                IsDeleted = false,
            };
            _context.Stores.Add(store);
            _context.SaveChanges();
            var sellerStore = new SellerStore
            {
                UserId = userId,
                StoreId = store.StoreId,
                Ismanager = isManager,
            };
            _context.SellerStores.Add(sellerStore);
            _context.SaveChanges();
            return store;
        }

        public async Task remove(string token, string storeId)
        {
            string userId = await getUserId(token);
            var x = _context.SellerStores.FirstOrDefault(s => s.UserId == userId && s.StoreId == storeId);
            if (x == null)
                throw new NotFoundException("This user does not own this store.");
            _context.SellerStores.Remove(x);
            _context.SaveChanges();

            // if all owners are removed, then delete the store from the table
            var count = _context.SellerStores.Count(s => s.StoreId == storeId);
            if(count == 0)
            {
                var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
                _context.Stores.Remove(store);
                _context.SaveChanges();
            }
        }

        public async Task<Store> invite(string token, string storeId)
        {
            string userId = await getUserId(token);
            var x = _context.SellerStores.FirstOrDefault(s => s.UserId == userId && s.StoreId == storeId);
            if (x != null)
                throw new DuplicateException("This user has already owned this store.");
            var sellerStore = new SellerStore
            {
                StoreId = storeId,
                UserId = userId,
            };
            _context.SellerStores.Add(sellerStore);
            _context.SaveChanges();
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            return store;
        }

        public async Task delete(string storeId)
        {
            var x = _context.SellerStores.FirstOrDefault(s => s.StoreId == storeId);
            if (x == null)
                throw new NotFoundException("This store does not exist");
            var stores = _context.SellerStores.Where(s => s.StoreId == storeId);
            _context.SellerStores.RemoveRange(stores);

            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            _context.Stores.Remove(store);
            _context.SaveChanges();
        }

        public async Task<SellerStore> setManager(string token, string storeId)
        {
            string userId = await getUserId(token);
            var sellerStore = _context.SellerStores.FirstOrDefault(s => s.StoreId == storeId && s.UserId ==userId);
            if (sellerStore == null)
                throw new NotFoundException("This user does not own this store.");
            sellerStore.Ismanager = 1;
            _context.SaveChanges();
            return sellerStore;
        }

        public async Task<IPage<Store>> getMyStore(int pageNo, int pageSize, string token)
        {
            string userId = await getUserId(token);
            var stores = _context.SellerStores.Where(s => s.UserId == userId).ToList();
            List<Store> result = new List<Store>();
            foreach (var store in stores)
            {
                var x = _context.Stores.FirstOrDefault(s => s.StoreId == store.StoreId);
                if (x == null)
                    throw new NotFoundException("This store does not exist.");
                result.Add(x);
            }

            var list = result.Skip((pageNo - 1) * pageSize)
              .Take(pageSize)
              .ToList();
            IPage<Store> Page = IPage<Store>.builder()
               .records(list)
               .total(result.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;
        }

        //public async Task<IPage<Store>> getStoreByName(int pageNo, int pageSize, string? storeName)
        //{
        //    List<Store> result = new List<Store>();
        //    if (storeName == null)
        //        result = _context.Stores.ToList();
        //    else
        //        result = _context.Stores.Where(s => s.StoreName.Contains(storeName)).ToList();

        //    var list = result.Skip((pageNo - 1) * pageSize)
        //      .Take(pageSize)
        //      .ToList();
        //    IPage<Store> Page = IPage<Store>.builder()
        //       .records(list)
        //       .total(result.Count)
        //       .size(pageSize)
        //       .current(pageNo)
        //       .build();
        //    return Page;

        //}
        public async Task<IPage<Store>> getPage(int pageNo, int pageSize, string? token, string? storeName)
        {
            List<Store> temp = new List<Store>();
            List<Store> result = new List<Store>();
            if (token == null)
                temp = _context.Stores.ToList();
            else
            {
                var userId = await getUserId(token);
                var stores = _context.SellerStores.Where(s => s.UserId == userId).ToList();
                foreach(var store in stores)
                {
                    var x = _context.Stores.FirstOrDefault(s => s.StoreId == store.StoreId);
                    if(x != null)
                        temp.Add(x);
                }
            }
            if(storeName == null)
                result = temp;
            else
                result = temp.Where(s => s.StoreName.Contains(storeName)).ToList();


            var list = result.Skip((pageNo - 1) * pageSize)
              .Take(pageSize)
              .ToList();
            IPage<Store> Page = IPage<Store>.builder()
               .records(list)
               .total(result.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;

        }
        public async Task<Store> getInfo(string storeId)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            return store;
        }

        public async Task<IPage<User>> getSeller(int pageNo, int pageSize, string storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s =>s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist");
            var result = _context.SellerStores.Where(s => s.StoreId == storeId).ToList();
            List<User> users = new List<User>();
            foreach (var item in result)
            {
                var x = _context.Users.FirstOrDefault(s => s.UserId == item.UserId);
                users.Add(x);
            }

            var list = users.Skip((pageNo - 1) * pageSize)
             .Take(pageSize)
             .ToList();
            IPage<User> Page = IPage<User>.builder()
               .records(list)
               .total(users.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;
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
                throw new NotFoundException("No match to this token.");

            return userId;
        }
    }
}
