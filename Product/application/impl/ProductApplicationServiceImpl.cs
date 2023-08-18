using Microsoft.AspNetCore.Mvc;
using Product.common;
using Product.domain.model;
using Product.domain.model.repository;
using Product.domain.service;
using Product.dto;
using Product.utils;

namespace Product.application.impl
{
    public class ProductApplicationServiceImpl : ProductApplicationService
    {
        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;
        private ProductService _productService;

        public ProductApplicationServiceImpl(ProductRepository productRepository, CategoryRepository categoryRepository,ProductService productService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productService = productService;
        }
      
        //Authorization:seller
        //首次创建某种商品，包含property,type信息
        public async Task<CommodityIdDto> createCommodity(CreateCommodityDto commodity)
        {
            var productAggregate = ProductAggregate.create(commodity);
            await _productRepository.add(productAggregate);
            return new CommodityIdDto {commodityId=productAggregate.ProductId};
        }

        //Authorization:seller
        //seller获得某一id的商品的所有信息
        public ProductAggregate getCommodity([FromBody] CommodityIdDto commodityId)
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
        }

        //Authorization:seller
        //对pick表更新 not batch
        public async Task updatePick(PickDto pick)
        {
            await _categoryRepository.setPick(pick.commodityId, pick.Filter,
                new MyFilterDto(pick.Change));
        }

        //Authorization:seller
        //对pick表更新 batch
        public async Task updatePick(List<PickDto> pick)
        {
            foreach(var it in pick)
            {
                await _categoryRepository.setPick(it.commodityId, it.Filter,
                new MyFilterDto(it.Change));
            }
            
        }

        //对某种商品的分页查询，不包含property，type信息
        public IPage<ProductAggregate> commodityPageQuery(PageQueryDto pageQuery)
        {
            return _productRepository.pageQuery(pageQuery);
        }

        //Authorization:seller
        public ProductAggregate getCommodityInfo(CommodityIdDto commodityId)
        {
            return _productRepository.getById(commodityId.commodityId);
        }




        //Authorization:buyer
        //某种商品的具体分类
        public List<IGrouping<string, DPick>> displayPicks(CommodityIdDto commodityId)
        {
            return _productService.displayPicks(commodityId);
        }


        //Authorization:seller
        //对某种商品的删除（全部删除）
        public async Task deleteCommodity(CommodityIdDto commodityId)
        {
            await _productRepository.delete(commodityId.commodityId);
        }






     
    }
}
