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
    public class StoreController : ControllerBase
    {
        public static StoreService storeService;
        public StoreController(StoreService _storeService)
        {
            storeService = _storeService;
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> add([FromHeader] string token, [FromBody] StoreAddDto storeAddDto)
        {
            var x = await storeService.add(token, storeAddDto.storeName, storeAddDto.isManager);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task remove([FromHeader] string token, [FromBody] StoreDelDto storeDelDto)
        {
            await storeService.remove(token, storeDelDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> invite([FromBody] StoreDelDto storeDelDto)
        {
            var x = await storeService.invite(storeDelDto.token, storeDelDto.StoreId);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task delete([FromBody] StoreDelDto storeDelDto)
        {
            await storeService.delete(storeDelDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<SellerStoreVo>> setManager([FromBody] StoreDelDto storeDelDto)
        {
            var x = await storeService.setManager(storeDelDto.token, storeDelDto.StoreId);
            return ComResponse<SellerStoreVo>.success(new SellerStoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getMyStore([FromHeader]string token, [FromBody]PageDto pageDto)
        {
            IPage<Store> x = await storeService.getMyStore(pageDto.pageNo, pageDto.pageSize, token);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getPage([FromBody]StoreGetPageDto storeGetPageDto)
        {
            IPage<Store> x = await storeService.getPage(storeGetPageDto.pageNo, storeGetPageDto.pageSize, storeGetPageDto.token, storeGetPageDto.storeName);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<Store>> getInfo([FromBody]StoreGetByIdDto storeGetByIdDto)
        {
            Store x = await storeService.getInfo(storeGetByIdDto.storeId);
            return ComResponse<Store>.success(x);   
        }

       [HttpPost]
       public async Task<ComResponse<IPage<User>>> getSeller([FromBody]UserGetByStoreDto userGetByStoreDto)
        {
            IPage<User> x = await storeService.getSeller(userGetByStoreDto.pageNo, userGetByStoreDto.pageSize,userGetByStoreDto.StoreId);
            return ComResponse<IPage<User>>.success(x);
        }
    }
}
