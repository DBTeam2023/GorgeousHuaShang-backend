using EntityFramework.Models;
using Storesys.utils;
using Microsoft.AspNetCore.Mvc;
namespace Storesys.service
{
    public interface StoreService
    {
        public Task<Store> add(string token, string storeName, int isManager, string? des, string? address, IFormFile? image);
        public Task remove(string token, string storeId);
        public Task<Store> invite(string userName, string storeId);
        public Task delete(string userName, string storeId);
        //public Task revise(string storeId, string? storeName, string? des, string? address, string? imgpath);
        public Task<SellerStore> setManager(string token, string storeId);
        public Task<Store> setScore(string storeId, decimal score);
        public Task<IPage<Store>> getMyStore(int pageNo, int pageSize, string token);
        public Task<IPage<Store>> getPage(int pageNo, int pageSize, string? token, string? storeName);
        public Task<Store> getInfo(string storeId);
        public Task<IPage<User>> getSeller(int pageNo, int pageSize, string storeId);
        public Task<Store> setDes(string storeId, string des);
        public Task<Store> setAddress(string storeId, string address);
        public Task<Store> addCollection(string token, string storeId);
        public Task<Store> removeCollection(string token, string storeId);
        public Task<IPage<Buyer>> getBuyer(int pageNo, int pageSize, string storeId);
        public Task<IPage<Store>> getCollection(int pageNo, int pageSize, string token, string? storeName);

        public Task<string> setAvatar(IFormFile? image, string imageName);
        public Task<FileContentResult?> getAvatar(string imageName);
    }
}
