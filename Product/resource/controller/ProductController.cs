using Microsoft.AspNetCore.Mvc;
using Product.domain.model.repository.impl;
using Product.common;
using Product.domain.model;
using EntityFramework.Context;
using Product.utils;
using Product.application;
using Product.dto;
using Product.resource.vo;
using Product.domain.service;
using Product.domain.service.impl;
using System.Text.Json;

namespace Product.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        //application service
        public ProductApplicationService productApplicationService;
        public ProductController(ProductApplicationService _productApplicationService)
        {
            productApplicationService = _productApplicationService;
        }

        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<CommodityIdDto>> createCommodity([FromForm] CreateCommodityAuxDto commodity)
        {
            return ComResponse<CommodityIdDto>.success(await productApplicationService.createCommodity(new CreateCommodityDto(commodity)));
        }


        //Authorization:seller
        [HttpPost]
        public ComResponse<ProductInfoVo> getCommodity([FromBody] CommodityIdDto commodityId)
        {
            var productAggrgate = productApplicationService.getCommodity(commodityId);
            return ComResponse<ProductInfoVo>.success(new ProductInfoVo(productAggrgate));
        }

        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<string>> updateCommodity([FromForm] CommodityAuxDto commodity)
        {
            await productApplicationService.updateCommodity(new CommodityDto(commodity));
            return ComResponse<string>.success("成功更新");
        }


        //Authorization:seller
        //update once a time
        [HttpPost]
        public async Task<ComResponse<string>> updatePick([FromForm] PickAuxDto pick)
        {
            await productApplicationService.updatePick(new PickDto(pick));
            return ComResponse<string>.success("成功更新");
        }
        ////Authorization:seller
        ////update batch
        //[HttpPost]
        //public async Task<ComResponse<string>> updatePickBatch([FromBody] List<PickDto> pickList)
        //{
        //    await productApplicationService.updatePick(pickList);
        //    return ComResponse<string>.success("成功更新");
        //}


        //Authorization:buyer seller
        //如果isdeleted==true，那么也显示这个商品，但是前端要标注已下架
        [HttpPost]
        public ComResponse<IPage<CommodityVo>> commodityPageQuery([FromBody] PageQueryDto pageQuery)
        {
            var page = productApplicationService.commodityPageQuery(pageQuery);
            return ComResponse<IPage<CommodityVo>>.success(CommodityVo.createCommodityPageVo(page));
        }


        [HttpPost]
        //Authorization:buyer
        //某种商品的具体分类
        public ComResponse<PickVo> displayPicks([FromBody] CommodityIdDto commodityId)
        {
            var picks = productApplicationService.displayPicks(commodityId);

            return ComResponse<PickVo>.success(new PickVo(picks));
        }



        //Authorization:seller
        //对某种商品的删除（全部删除）
        [HttpPost]
        public async Task<ComResponse<string>> deleteCommodity([FromBody] CommodityIdDto commodityId)
        {
            await productApplicationService.deleteCommodity(commodityId);
            return ComResponse<string>.success("成功删除");
        }


        //test jsonConvertService

        //[HttpPost]
        //public ComResponse<Dictionary<string,List<string>>> test(string str)
        //{
        //    var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(str);
        //    return ComResponse<Dictionary<string, List<string>>>.success(dict);
        //}







    }
}
