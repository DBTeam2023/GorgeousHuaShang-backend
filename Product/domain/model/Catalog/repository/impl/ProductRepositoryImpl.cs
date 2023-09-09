using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Product.dto;
using Product.exception;
using Product.utils;

namespace Product.domain.model.repository.impl
{
    public class ProductRepositoryImpl : ProductRepository
    {
        private readonly ModelContext _context;

        private CategoryRepository _categoryRepository;

        public ProductRepositoryImpl(ModelContext context, CategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        //exception fixed
        public async Task add(ProductAggregate productAggregate)
        {
            var exist = _context.CommodityGenerals.Where(c => c.CommodityId == productAggregate.ProductId).FirstOrDefault();
            if (exist != null)
                throw new DuplicateException("The commodity has already existed");
            IDbContextTransaction? tran = null;

            var new_product = new CommodityGeneral
            {
                CommodityId = productAggregate.ProductId,
                StoreId = productAggregate.StoreId,
                Description = productAggregate.Description,
                CommodityName = productAggregate.ProductName,
                IsDeleted = productAggregate.IsDeleted,
                Price = productAggregate.Price
            };
            try
            {
                tran = _context.Database.BeginTransaction();
                await _context.CommodityGenerals.AddAsync(new_product);
                await _context.SaveChangesAsync();
                //交给category资源库处理
                
                await _categoryRepository.addNoTransaction(productAggregate.Category);
                //commit transaction
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                throw new DBFailureException("add product faliure");
            }
            finally
            {
                tran.Dispose();
            }


        }

        //exception fixed
        public async Task update(ProductAggregate productAggregate)
        {
            var db_product = _context.CommodityGenerals.Where(c => c.CommodityId == productAggregate.ProductId).FirstOrDefault();
            if (db_product == null)
                throw new NotFoundException("the commodity doesn't exist");

            //change
            db_product.CommodityName = productAggregate.ProductName;
            db_product.Description = productAggregate.Description;
            db_product.IsDeleted = productAggregate.IsDeleted;
            db_product.Price = productAggregate.Price;
            db_product.StoreId = productAggregate.StoreId;

            IDbContextTransaction? tran = null;
            try
            {
                tran = _context.Database.BeginTransaction();
                await _context.SaveChangesAsync();
                //交给category资源库update
                
                await _categoryRepository.updateNoTransaction(productAggregate.Category);
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                throw new DBFailureException("update product faliure");
            }
            finally
            {
                tran.Dispose();
            }


        }

        //exception fixed
        public ProductAggregate getById(string commodityId)
        {
            var db_product = _context.CommodityGenerals.Where(c => c.CommodityId == commodityId).FirstOrDefault();
            if (db_product == null)
                throw new NotFoundException("The commodity doesn't exist");

            var product_model = new ProductAggregate(db_product.StoreId,
                db_product.CommodityId, db_product.CommodityName, db_product.Price,
                db_product.Description, _categoryRepository.getById(commodityId),
                db_product.IsDeleted);

            return product_model;

        }

        //exception fixed
        public async Task delete(string commodityId)
        {
            //commodityGeneral
            var commodity_general = _context.CommodityGenerals.Where(c => c.CommodityId == commodityId).FirstOrDefault();
            if (commodity_general == null)
                throw new NotFoundException("The commodity doesn't exist");

            _context.CommodityGenerals.Remove(commodity_general);
            //其余交给category资源库处理
            //无需处理：DB会级联删除
            //try catch 仅保险起见
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("delete product faliure");
            }

        }



        private List<ProductAggregate> Convert(List<CommodityGeneral> x)
        {
            var ans = new List<ProductAggregate>();
            foreach (var product in x)
            {
                ans.Add(new ProductAggregate(product.StoreId, product.CommodityId,
                    product.CommodityName, product.Price,
                    product.Description, null, product.IsDeleted));
            }
            return ans;
        }



        //should be written in ProductRepository and ProductRepositoryImpl
        public IPage<ProductAggregate> pageQuery(PageQueryDto pageQuery)
        {
            //page query now has arguments below
            //@arguments        @query
            //commodityId       equal
            //storeId           equal
            //pricemin          range
            //pricemax          range
            //type              like
            //name              like
            //desription        like
            pageQuery.check();
            var all = _context.CommodityGenerals.ToList();




            var ans = _context.CommodityGenerals
                .Where(x => x.CommodityId == (pageQuery.CommodityId ?? x.CommodityId))
                .Where(x => x.StoreId == (pageQuery.StoreId ?? x.StoreId))
                .Where(x => x.CommodityName.Contains((pageQuery.Name ?? "")))
                .Where(x => x.Price >= (pageQuery.Pricemin ?? decimal.MinValue))
                .Where(x => x.Price <= (pageQuery.Pricemax ?? decimal.MaxValue)).ToList();


            if (pageQuery.Description != null)
                ans = ans.Where(x => x.Description != null && x.Description.Contains((pageQuery.Description ?? ""))).ToList();

            var ans_type = _context.Categories.ToList();
               

            if(pageQuery.Type != null)
                ans_type= ans_type.Where(x => x.Type != null && x.Type.Contains((pageQuery.Type ?? ""))).ToList();

            var result = ans.Join(ans_type,
                 ansItem => ansItem.CommodityId,
                 ansTypeItem => ansTypeItem.CommodityId,
                 (ansItem, ansTypeItem) => ansItem).ToList();

            int page_index = pageQuery.PageIndex;
            int page_size = pageQuery.PageSize;
            int total = result.Count();
            
            
            if(total >= (page_index-1)*page_size+1)
            {
                if(total <= page_index*page_size)
                    result = result.GetRange((page_index - 1) * page_size, result.Count() - (page_index - 1) * page_size);
                else
                    result = result.GetRange((page_index - 1) * page_size, page_size);
            }
            else//page not found
            {
                if(!(total==0&&page_index==1))
                    throw new PageException("Page not found");
            }
                



            var page = IPage<ProductAggregate>.builder().total(total)
                .size(page_size).current(page_index).records(Convert(result)).build();

            return page;

        }





    }
}
