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
        public async Task<ComResponse<StoreVo>> add([FromForm] StoreAddDto storeAddDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            var x = await storeService.add(token, storeAddDto.storeName, storeAddDto.isManager, storeAddDto.des, storeAddDto.address, storeAddDto.image);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task remove([FromBody] StoreDelDto storeDelDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            await storeService.remove(token, storeDelDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> invite([FromBody] StoreRemoveDto storeRemoveDto)
        {
            var x = await storeService.invite(storeRemoveDto.userName, storeRemoveDto.StoreId);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task delete([FromBody] StoreRemoveDto storeRemoveDto)
        {
            await storeService.delete(storeRemoveDto.userName, storeRemoveDto.StoreId);
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> setScore([FromBody] ScoreSetDto scoreSetDto)
        {
            var x = await storeService.setScore(scoreSetDto.storeId, scoreSetDto.score);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<SellerStoreVo>> setManager([FromBody] StoreRemoveDto storeRemoveDto)
        {
            var x = await storeService.setManager(storeRemoveDto.userName, storeRemoveDto.StoreId);
            return ComResponse<SellerStoreVo>.success(new SellerStoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getMyStore([FromBody] PageDto pageDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            IPage<Store> x = await storeService.getMyStore(pageDto.pageNo, pageDto.pageSize, token);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getPage([FromBody] StoreGetPageDto storeGetPageDto)
        {
            IPage<Store> x = await storeService.getPage(storeGetPageDto.pageNo, storeGetPageDto.pageSize, storeGetPageDto.token, storeGetPageDto.storeName);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<Store>> getInfo([FromBody] StoreGetByIdDto storeGetByIdDto)
        {
            Store x = await storeService.getInfo(storeGetByIdDto.storeId);
            return ComResponse<Store>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<User>>> getSeller([FromBody] UserGetByStoreDto userGetByStoreDto)
        {
            IPage<User> x = await storeService.getSeller(userGetByStoreDto.pageNo, userGetByStoreDto.pageSize, userGetByStoreDto.StoreId);
            return ComResponse<IPage<User>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> setDes([FromBody] DesSetDto desSetDto)
        {
            Store x = await storeService.setDes(desSetDto.storeId, desSetDto.des);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> setAddress([FromBody] AddressSetDto addressSetDto)
        {
            Store x = await storeService.setAddress(addressSetDto.storeId, addressSetDto.address);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> addCollection([FromBody] CollectionDto collectionDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            Store x = await storeService.addCollection(token, collectionDto.storeId);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<StoreVo>> removeCollection([FromBody] CollectionDto collectionDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            Store x = await storeService.removeCollection(token, collectionDto.storeId);
            return ComResponse<StoreVo>.success(new StoreVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Store>>> getCollection([FromBody] CollectionGetDto collectionGetDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            IPage<Store> x = await storeService.getCollection(collectionGetDto.pageNo, collectionGetDto.pageSize, token, collectionGetDto.storeName);
            return ComResponse<IPage<Store>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Buyer>>> getBuyer([FromBody] BuyerGetDto buyerGetDto)
        {
            IPage<Buyer> x = await storeService.getBuyer(buyerGetDto.pageNo, buyerGetDto.pageSize, buyerGetDto.storeId);
            return ComResponse<IPage<Buyer>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<string>> setAvatar([FromForm] AvatarSetDto avatarSetDto)
        {
            string x = await storeService.setAvatar(avatarSetDto.image, avatarSetDto.imageName);
            return ComResponse<string>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<FileContentResult?>> getAvatar([FromBody] StoreGetByIdDto storeGetByIdDto)
        {
            var x = await storeService.getAvatar(storeGetByIdDto.storeId);
            return ComResponse<FileContentResult?>.success(x);
        }

    }
}
