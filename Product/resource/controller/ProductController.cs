using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using Product.application;
//using Product.domain.model;
//using Product.domain.service;
//using Product.dto;
//using Product.exception;
//using Product.resource.vo;
//using Product.utils;
using Product.domain.model.repository.impl;
using Product.common;
using Product.domain.model;
using EntityFramework.Context;
using Product.utils;
using Product.application;
using Product.dto;
using Product.resource.vo;

/**
 * @author sty
 * @implnote use "[action]" in URL for allowing asynchronous http request
 */
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
        public async Task<ComResponse<CommodityIdDto>> createCommodity([FromBody] CreateCommodityDto commodity)
        {          
            return ComResponse<CommodityIdDto>.success(await productApplicationService.createCommodity(commodity));
        }

        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<ProductInfoVo>> getCommodity([FromBody] CommodityIdDto commodityId)
        {
            var productAggrgate=productApplicationService.getCommodity(commodityId);
            return ComResponse<ProductInfoVo>.success(new ProductInfoVo(productAggrgate));

        }



        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<string>> updateCommodity([FromBody] CommodityDto commodity)
        {
            await productApplicationService.updateCommodity(commodity);
            return ComResponse<string>.success("成功更新");
        }


        //Authorization:seller
        //update once a time
        [HttpPost]
        public async Task<ComResponse<string>> updatePick([FromBody] PickDto pick)
        {
            await productApplicationService.updatePick(pick);
            return ComResponse<string>.success("成功更新");
        }
        //Authorization:seller
        //update batch
        [HttpPost]
        public async Task<ComResponse<string>> updatePickBatch([FromBody] List<PickDto> pickList)
        {
            await productApplicationService.updatePick(pickList);
            return ComResponse<string>.success("成功更新");
        }


        //Authorization:buyer seller
        //如果isdeleted==true，那么也显示这个商品，但是前端要标注已下架
        [HttpPost]
        public ComResponse<IPage<CommodityVo>> commodityPageQuery([FromBody]PageQueryDto pageQuery)
        {
            var page = productApplicationService.commodityPageQuery(pageQuery);
            return ComResponse<IPage<CommodityVo>>.success(CommodityVo.createCommodityPageVo(page));
        }


        [HttpPost]
        //Authorization:buyer
        //某种商品的具体分类
        public ComResponse<List<PickVo>> displayPicks([FromBody] CommodityIdDto commodityId)
        {
            var picks=productApplicationService.displayPicks(commodityId);
            
            return ComResponse<List<PickVo>>.success(PickVo.createPickVo(picks));
        }



        //Authorization:seller
        //对某种商品的删除（全部删除）
        [HttpPost]
        public async Task<ComResponse<string>> deleteCommodity([FromBody] CommodityIdDto commodityId)
        {
            await productApplicationService.deleteCommodity(commodityId);
            return ComResponse<string>.success("成功删除");
        }















        //[HttpPost]
        //public async Task add()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    var property = new Dictionary<string, List<string>>()
        //    {
        //        {"color",new List<string>{"red","blue","yellow"} },
        //        {"size",new List<string>{"big","small"} },
        //        {"made",new List<string>{"silk","paper","cloth"} }
        //    };

        //    var aggregate = new CategoryAggregate("95533", new List<DPick>(), property, new TrousersType());
        //    await cat.add(aggregate);


        //}
        //[HttpPost]
        //public async Task setPick()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());

        //    var filter = new Dictionary<string, string>()
        //    {
        //        {"color","red" },
        //        {"made","cloth" }
        //    };
        //    var change = new Dictionary<string, string?>()
        //    {
        //        {"isDeleted","y"},
        //        {"description","newly add" },
        //        {"price","224.34" }

        //    };
        //    await cat.setPick("95533", filter, new dto.MyFilter(change));
        //}



        //[HttpPost]
        //public async Task delete()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    await cat.delete("95533");
        //}

        //[HttpPost]
        //public ComResponse<IPage<ProductAggregate>> page(int x)
        //{
        //    ProductRepositoryImpl pro = new ProductRepositoryImpl(new ModelContext(), new CategoryRepositoryImpl(new ModelContext()));
        //    var ans = pro.pageQuery(new dto.PageQueryDto(3, x, new Dictionary<string, string>()
        //    {
        //        {"name" ,"啊"},
        //        {"description" ,"啊"},
        //        { "pricemin","30"},
        //        { "pricemax","90"}

        //        }));
        //    return ComResponse<IPage<ProductAggregate>>.success(ans);
        //}



        //[HttpPost]
        //public async Task updatePick()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    var group_pick=cat.setAvailable("95533",new Dictionary<string, string>()
        //    {
        //        {"color","red" },
        //        {"made","silk" }
        //    });
        //    var ori_pick = cat.getById("95533").DetailPicks;
        //    var x = ori_pick.Except(group_pick.ToList());


        //}




        //[HttpPost]
        //public async Task update()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    var property = new Dictionary<string, List<string>>()
        //    {
        //        {"color",new List<string>{"red","blue"} },
        //        {"size",new List<string>{"big","small"} },
        //        {"made",new List<string>{"silk","cloth","paper"} }

        //    };

        //    var aggregate = new CategoryAggregate("95533", new List<DPick>(), property, new TshirtType());
        //    await cat.update(aggregate);
        //}

        //[HttpPost]
        //public async Task setBatch()
        //{
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    cat.setAvailable(new Dictionary<string, string>()
        //    {
        //        {"color","red" },
        //        {"made","silk" }
        //    });
        //}


        //public async Task pageQuery()
        //{
        //    //commodityId       equal
        //    //storeId           equal
        //    //pricemin          range
        //    //pricemax          range
        //    //type              like
        //    //name              like
        //    //desription        like
        //    CategoryRepositoryImpl cat = new CategoryRepositoryImpl(new ModelContext());
        //    cat.pageQuery2(new dto.PageQueryDto(2, 3, new Dictionary<string, string>()
        //    {

        //    })); ;
        //}





        //[HttpPost]
        //public ComResponse<TokenDto> register([FromBody] RegisterDto user)
        //{
        //    return ComResponse<TokenDto>.success(ProductService.register(user.Username, user.Password, user.Type));
        //}

        //[HttpPost]
        //public ComResponse<int> update([FromBody] UserAggregate userAggregate)
        //{
        //    ProductService.update(userAggregate);

        //    return ComResponse<int>.success(0);
        //}

        //[HttpGet]
        //public async Task<ComResponse<UserAggregate>> getUserInfo()
        //{
        //    //resolve token
        //    string token = Request.Headers["Authorization"].ToString();
        //    if (token == null)
        //    {
        //        throw new ParamMissingException("token required");
        //    }
        //    var newToken = token.Replace("Bearer ", "");
        //    var username = jwt.resolveToken(newToken);

        //    return ComResponse<UserAggregate>.success(ProductService.getUserInfoByUsername(username));
        //}
    }
}
