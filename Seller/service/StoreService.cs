using EntityFramework.Models;
using Storesys.utils;
namespace Storesys.service
{
    public interface StoreService
    {
        public Task<Store> add(string token, string storeName, int isManager);
        public Task remove(string token, string storeId);
        public Task<Store> invite(string token, string storeId);
        public Task delete(string storeId);
        public Task<SellerStore> setManager(string token, string storeId);
        public Task<IPage<Store>> getStoreBySeller(int pageNo, int pageSize, string token);
        public Task<IPage<Store>> getStoreByName(int pageNo, int pageSize, string? storeName);
        public Task<Store> getStoreById(string storeId);
        public Task<IPage<User>> getUserByStore(int pageNo, int pageSize, string storeId);
    }
}
