using EntityFramework.Context;
using EntityFramework.Models;
using Storesys.exception;
using Storesys.utils;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Storesys.utils;
using Newtonsoft.Json.Linq;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<Store> add(string token, string storeName, int isManager, string? des, string? address, IFormFile? image)
        {
            string userId = await getUserId(token);
            var seller = _context.Sellers.FirstOrDefault(s => s.UserId == userId);
            if (seller == null)
                throw new NotFoundException("This user is not a seller.");
            if (des == null)
                des = "这家店的主人很懒，暂时没有描述哦";
            if (address == null)
                address = "上海 嘉定";
            var store = new Store
            {
                StoreId = GenerateRandomString(10),
                StoreName = storeName,
                Score = 5,
                IsDeleted = false,
                Description = des,
                Address = address,
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
            if (image != null)
                await setAvatar(image, store.StoreId);
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

        public async Task<Store> invite(string userName, string storeId)
        {
            var user = _context.Users.FirstOrDefault(s => s.Username == userName);
            if (user == null)
                throw new NotFoundException("This user does not exist.");
            if (user.Type == "buyer")
                throw new NotFoundException("This user is a buyer.");
            string userId = user.UserId;
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

        public async Task delete(string userName, string storeId)
        {
            var user = _context.Users.FirstOrDefault(s => s.Username == userName);
            if (user == null)
                throw new NotFoundException("This user does not exist.");
            string userId = user.UserId;
            var x = _context.SellerStores.FirstOrDefault(s => s.UserId == userId && s.StoreId == storeId);
            if (x == null)
                throw new NotFoundException("This seller does not own this store.");
            _context.SellerStores.Remove(x);
            await _context.SaveChangesAsync();
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

        public async Task<Store> setScore(string storeId, decimal score)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            store.Score = score;
            await _context.SaveChangesAsync();
            return store;
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
        public async Task<Store> setDes(string storeId, string des)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            store.Description = des;
            await _context.SaveChangesAsync();
            return store;
        }
        public async Task<Store> setAddress(string storeId, string address)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            store.Address = address;
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Store> addCollection(string token, string storeId)
        {
            string userId = await getUserId(token);
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            var buyerstore = new BuyerStore
            {
                UserId = userId,
                StoreId = storeId,
                Hasbought = 0,
            };
            _context.BuyerStores.Add(buyerstore);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Store> removeCollection(string token, string storeId)
        {
            string userId = await getUserId(token);
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            var buyerstore = new BuyerStore
            {
                UserId = userId,
                StoreId = storeId,
            };
            var x = _context.BuyerStores.FirstOrDefault(s => s.UserId == userId && s.StoreId == storeId);
            if (x == null)
                throw new NotFoundException("This store has not been collected by the buyer.");
            _context.BuyerStores.Remove(x);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<IPage<Store>> getCollection(int pageNo, int pageSize, string token, string? storeName)
        {
            string userId = await getUserId(token);
            var buyer = _context.Buyers.FirstOrDefault(s => s.UserId == userId);
            if (buyer == null)
                throw new NotFoundException("This user is not a buyer.");

            List<Store> result = new List<Store>();
            List<Store> temp = new List<Store>();
            var stores = _context.BuyerStores.Where(s => s.UserId == userId).ToList();
            foreach (var store in stores)
            {
                var x = _context.Stores.FirstOrDefault(s => s.StoreId == store.StoreId);
                if (x == null)
                    throw new NotFoundException("This store does not exist.");
                temp.Add(x);
            }
            if (storeName != null)
                result = temp.Where(s => s.StoreName.Contains(storeName)).ToList();
            else
                result = temp;

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

        public async Task<IPage<Buyer>> getBuyer(int pageNo, int pageSize, string storeId)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
                throw new NotFoundException("This store does not exist.");
            List<Buyer> buyers = new List<Buyer>();
            var buyerids = _context.BuyerStores.Where(s =>s.StoreId == store.StoreId).ToList();
            foreach(var buyerid in buyerids)
            {
                var x = _context.Buyers.FirstOrDefault(s => s.UserId == buyerid.UserId);
                buyers.Add(x);
            }

            var list = buyers.Skip((pageNo - 1) * pageSize)
             .Take(pageSize)
             .ToList();
            IPage<Buyer> Page = IPage<Buyer>.builder()
               .records(list)
               .total(buyers.Count)
               .size(pageSize)
               .current(pageNo)
               .build();
            return Page;
        }

        public async Task<string> setAvatar(IFormFile? image, string imageName)
        {
            ImageService imageService = new ImageService("C:/Users/Administrator/Desktop/image/Store/");
            // ImageService imageService = new ImageService("C:/Users/86134/Desktop/test/");
            string str = await imageService.setImage(image, imageName);
            return str;
        }

        public async Task<FileContentResult?> getAvatar(string storeId)
        {
            ImageService imageService = new ImageService("C:/Users/Administrator/Desktop/image/Store/");
            // ImageService imageService = new ImageService("C:/Users/86134/Desktop/test/");
            var img = await imageService.getImage(storeId);
            return img;
        }

        public async Task<string> getUserId(string token)
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
                throw new NotFoundException("No match to this token.");

            return userId;
        }
    }
}
