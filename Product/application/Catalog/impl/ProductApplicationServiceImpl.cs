using Microsoft.AspNetCore.Mvc;
using Product.common;
using Product.domain.model;
using Product.domain.model.repository;
using Product.domain.service;
using Product.domain.service.impl;
using Product.dto;
using Product.utils;

namespace Product.application.impl
{
    public class ProductApplicationServiceImpl : ProductApplicationService
    {
        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;
        private ProductService _productService;
        private AvatarService _avatarService;

        public ProductApplicationServiceImpl(ProductRepository productRepository,
            CategoryRepository categoryRepository,ProductService productService,
            AvatarService avatarService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productService = productService;
            _avatarService = avatarService;
        }
      
        //Authorization:seller
        //首次创建某种商品，包含property,type信息
        public async Task<CommodityIdDto> createCommodity(CreateCommodityDto commodity)
        {
            var productAggregate = ProductAggregate.create(commodity);
            await _productRepository.add(productAggregate);
            _avatarService.setCommodityAvatar(commodity.image, productAggregate.ProductId);
            return new CommodityIdDto {commodityId=productAggregate.ProductId};
        }

        //Authorization:seller
        //seller获得某一id的商品的所有信息
        public ProductAggregate getCommodity(CommodityIdDto commodityId)
        {
            var productAggregate = _productRepository.getById(commodityId.commodityId);
            

            return productAggregate;
        }

        //Authorization:seller
        //对某种商品进行更新,包含property,type信息
        public async Task updateCommodity(CommodityDto commodity)
        {
            var productAggregate = ProductAggregate.create(commodity);
            await _productRepository.update(productAggregate);
            _avatarService.setCommodityAvatar(commodity.image, productAggregate.ProductId);
        }

        //Authorization:seller
        //对pick表更新 not batch
        public async Task updatePick(PickDto pick)
        {
            var picks = _categoryRepository.getPicks(pick);
            await _categoryRepository.setPick(pick);
            foreach (var item in picks)
                _avatarService.setPickAvatar(pick.image, item.Key);          
            
        }

        ////Authorization:seller
        ////对pick表更新 batch
        //public async Task updatePick(List<PickDto> pick)
        //{
        //    foreach (var it in pick)
        //    {
        //        await _categoryRepository.setPick(it);
        //    }

        //}

        //对某种商品的分页查询，不包含property，type信息
        public IPage<ProductAggregate> commodityPageQuery(PageQueryDto pageQuery)
        {
            var ans = _productRepository.pageQuery(pageQuery);        
            return ans;
        }



        //Authorization:buyer
        //某种商品的具体分类
        public List<IGrouping<string, DPick>> displayPicks(CommodityIdDto commodityId)
        {
            var picks= _productService.displayPicks(commodityId);

            return picks;
        }


        //Authorization:seller
        //对某种商品的删除（全部删除）
        public async Task deleteCommodity(CommodityIdDto commodityId)
        {
            var all_picks = _productRepository.getById(commodityId.commodityId);

            await _productRepository.delete(commodityId.commodityId);

            _avatarService.deleteCommodityAvatar(commodityId.commodityId);

            foreach (var pick in all_picks.Category.DetailPicks)
                _avatarService.deletePickAvatar(pick.PickId);
            
        }







    }
}
