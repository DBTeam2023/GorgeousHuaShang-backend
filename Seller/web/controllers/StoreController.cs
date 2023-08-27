using Microsoft.AspNetCore.Mvc;
using Storesys.core.dto;
using Storesys.core.vo;
using Storesys.service;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Storesys.utils;
namespace Storesys.web.controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StoreController: ControllerBase
    {
        public static StoreService storeService;
        public StoreController(StoreService  _storeService)
        {
            storeService = _storeService;
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> add([FromHeader]string token, [FromBody]StoreAddDto storeAddDto)
        {
            var x = await storeService.add(token, storeAddDto.storeName, storeAddDto.isManager);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }
        
        [HttpPost]
        public async Task remove([FromHeader]string token, [FromBody] StoreDelDto storeDelDto)
        {
            await storeService.remove(token, storeDelDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> invite([FromHeader]string token, [FromBody]StoreDelDto storeDelDto)
        {
            var x = await storeService.invite(token, storeDelDto.StoreId);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task delete([FromBody]StoreDelDto storeDelDto)
        {
            await storeService.delete(storeDelDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<SellerStoreVo>> setManager([FromHeader]string token, [FromBody] StoreDelDto storeDelDto)
        {
            var x = await storeService.setManager(token, storeDelDto.StoreId);
            return ComResponse<SellerStoreVo>.success(new SellerStoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getStoreBySeller([FromHeader]string token, [FromBody]StoreGetBySellerDto storeGetBySellerDto)
        {
            IPage<Store> x = await storeService.getStoreBySeller(storeGetBySellerDto.pageNo, storeGetBySellerDto.pageSize, token);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getStoreByName([FromBody]StoreGetByNameDto storeGetByNameDto)
        {
            IPage<Store> x = await storeService.getStoreByName(storeGetByNameDto.pageNo, storeGetByNameDto.pageSize, storeGetByNameDto.storeName);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<Store>> getStoreById([FromBody]StoreGetByIdDto storeGetByIdDto)
        {
            Store x = await storeService.getStoreById(storeGetByIdDto.storeId);
            return ComResponse<Store>.success(x);   
        }

       [HttpPost]
       public async Task<ComResponse<IPage<User>>> getUserByStore([FromBody]UserGetByStoreDto userGetByStoreDto)
        {
            IPage<User> x = await storeService.getUserByStore(userGetByStoreDto.pageNo, userGetByStoreDto.pageSize,userGetByStoreDto.StoreId);
            return ComResponse<IPage<User>>.success(x);
        }
    }
}
