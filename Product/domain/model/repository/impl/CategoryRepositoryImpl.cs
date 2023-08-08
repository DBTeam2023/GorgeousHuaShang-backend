using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Product.common;
using Product.exception;
namespace Product.domain.model.repository.impl
{
    public class CategoryRepositoryImpl
    {
        private readonly ModelContext _context;

        public CategoryRepositoryImpl(ModelContext context)
        {
            _context = context;
        }

        

        private static Category transferCategory(CategoryAggregate categoryAggregate)
        {
            //category 表
            var db_add_classification = new Category
            {
                CommodotyId = categoryAggregate.ProductId,
                Type = categoryAggregate.ClassficationType == null?null:BasicSortType.getAns(categoryAggregate.ClassficationType),
            };

            return db_add_classification;
        }
        private static List<CommodityProperty> transferProperty(CategoryAggregate categoryAggregate)
        {
            var db_add_property = new List<CommodityProperty>();
            foreach (KeyValuePair<string, List<string>> property in categoryAggregate.Property)
            {
                foreach (var val in property.Value)
                {
                    var new_property = new CommodityProperty
                    {
                        CommodityId = categoryAggregate.ProductId,
                        PropertyType = property.Key,
                        PropertyValue = val,
                    };
                    db_add_property.Add(new_property);
                }
            }
            return db_add_property;
        }

        private static List<Pick> transferPick(CategoryAggregate categoryAggregate)
        {
            var db_add_picks = new List<Pick>();
            foreach (DPick pick in categoryAggregate.DetailPicks)
            {
                var new_pick = new Pick
                {
                    PickId = pick.PickId,
                    Description = pick.Description,
                    CommodityId = categoryAggregate.ProductId,
                    IsDeleted = pick.IsDeleted,
                    Price = pick.Price,
                    PropertyType = pick.PropertyType,
                    PropertyValue = pick.PropertyValue,
                };
            }
            return db_add_picks;
        }

        //ok
        public async Task add(CategoryAggregate categoryAggregate)
        {
            //transaction begin
            IDbContextTransaction? tran = null;
            int exception = 0;
            //如果根本没有这个commodityId
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == categoryAggregate.ProductId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("you still don't have the commodity");
            try
            {
                tran = _context.Database.BeginTransaction();

                //property 表
                var db_add_property = transferProperty(categoryAggregate);
                exception = 1;
                
                await _context.CommodityProperties.AddRangeAsync(db_add_property);
                await _context.SaveChangesAsync();
                
                

                //pick 表
                var db_add_picks = transferPick(categoryAggregate);
                exception = 2;
                
                await _context.Picks.AddRangeAsync(db_add_picks);
                await _context.SaveChangesAsync();
                
                

                //category 表
                var db_add_classification = transferCategory(categoryAggregate);
                exception = 3;
                
                await _context.Categories.AddAsync(db_add_classification);
                await _context.SaveChangesAsync();
                
                
                exception = 4;
                //transaction commit
                await tran.CommitAsync();
            }

            catch
            {
                if (tran != null)
                    tran.Rollback();
                switch(exception)
                {
                    case 0:
                        throw;
                    case 1:
                        throw new DuplicateException("same property occurred");
                    case 2:
                        throw new DuplicateException("same pick occurred");
                    case 3:
                        throw new DuplicateException("category already exists");
                    default:
                        throw new TransactionCommitException("transaction commit failure.Ready to rollback");
                }               
            }
            finally
            {
                tran.Dispose();
            }
        }

        //T is dataaccess/DBModels
        internal List<T> difference<T>(List<T> ori,List<T> cur)
        {
            var ans = new List<T>();
            ans = ori.Except(cur).ToList();        
            return ans;
        }

      
        public async Task update(CategoryAggregate categoryAggregate)
        {           
            //如果根本没有这个commodityId
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == categoryAggregate.ProductId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("you still don't have the commodity");
            IDbContextTransaction? tran = null;
            int exception = 0;
            try
            {
                tran = _context.Database.BeginTransaction();
                //property 表
                var db_update_property = transferProperty(categoryAggregate);
                var db_ori_property = _context.CommodityProperties.Where(x => x.CommodityId == categoryAggregate.ProductId).ToList();
                var remove_old_property = difference<CommodityProperty>(db_ori_property, db_update_property);
                _context.CommodityProperties.RemoveRange(remove_old_property);
                var add_new_property = difference<CommodityProperty>(db_update_property, db_ori_property);
                _context.CommodityProperties.AddRange(add_new_property);
                await _context.SaveChangesAsync();
                exception = 1;

                //pick
                var db_update_pick = transferPick(categoryAggregate);
                var db_ori_pick = _context.Picks.Where(x => x.CommodityId == categoryAggregate.ProductId).ToList();
                var remove_old_pick = difference<Pick>(db_ori_pick, db_update_pick);
                _context.Picks.RemoveRange(remove_old_pick);
                var add_new_pick = difference<Pick>(db_update_pick, db_ori_pick);
                _context.Picks.AddRange(add_new_pick);
                
                await _context.SaveChangesAsync();
                exception = 2;


                //category
                var db_update_category = transferCategory(categoryAggregate);
                var db_ori_category = _context.Categories.Where(x => x.CommodotyId == categoryAggregate.ProductId).FirstOrDefault();
                //db_ori_category不可能为空
                db_ori_category.Type = db_update_category.Type;
                
                await _context.SaveChangesAsync();
                exception = 3;
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                switch (exception)
                {
                   
                    case 0:
                        throw new DuplicateException("same property occurred");
                    case 1:
                        throw new DuplicateException("same pick occurred");
                    case 3:
                        throw new DuplicateException("category already exists");
                    default:
                        throw new TransactionCommitException("transaction commit failure.Ready to rollback");
                }
            }
            finally
            {
                tran.Dispose();
            }
           
        }

        //ok
        public CategoryAggregate getById(string commodityId)
        {
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == commodityId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("The commodity does not exist");


            var db_property = _context.CommodityProperties.Where(x => x.CommodityId == commodityId);
            if (db_property.Count() != 0)
                db_property= db_property.OrderBy(x => x.PropertyType);                    
            var domain_property = new List<KeyValuePair<string, List<string>>>();
            var value = new List<string>();
            string? past_type = null;
            foreach (var it in db_property)
            {
                if (it.PropertyType != past_type)
                {
                    if(past_type!=null)//ignore the first time
                        domain_property.Add(new KeyValuePair<string, List<string>>(it.PropertyType, value));
                    past_type = it.PropertyType;
                    value.Clear();
                }
                value.Add(it.PropertyValue);                         
            }
            //compensate for the last time
            if (past_type != null)
                domain_property.Add(new KeyValuePair<string, List<string>>(past_type, value));


            var db_category= _context.Categories.Where(x => x.CommodotyId == commodityId).FirstOrDefault();

            BasicSortType? domain_category = (db_category == null ? null : BasicSortType.getFinalType(db_category.Type));
            


            var db_pick = _context.Picks.Where(x => x.CommodityId == commodityId);

            var domain_pick = new List<DPick>();
            foreach(var it in db_pick)
            {
                domain_pick.Add(new DPick
                {
                    CommodityId = it.CommodityId,
                    Description = it.Description,
                    IsDeleted = it.IsDeleted,
                    PickId = it.PickId,
                    Price = it.Price,
                    PropertyType = it.PropertyType,
                    PropertyValue = it.PropertyValue
                });
            }

            CategoryAggregate ans = new CategoryAggregate
            {
                ProductId = commodityId,
                Property = domain_property,
                DetailPicks = domain_pick,
                ClassficationType = domain_category,
            };

            return ans;



        }


        //ok
        public async Task delete(string commodityId)
        {
            var categoryAggregate = getById(commodityId);

            try
            {
                //property 表
                var db_add_property = transferProperty(categoryAggregate);

                _context.CommodityProperties.RemoveRange(db_add_property);


                //pick表将会级联删除，无须操作

                //category 表
                var db_add_classification = transferCategory(categoryAggregate);

                _context.Categories.Remove(db_add_classification);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DeleteException("delete failure");
            }
            
        }


    }
}
