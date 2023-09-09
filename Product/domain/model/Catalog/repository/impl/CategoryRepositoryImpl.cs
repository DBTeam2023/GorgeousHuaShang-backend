using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Product.common;
using Product.exception;
using Product.utils;
using Product.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Product.domain.model.repository.impl
{
    public class CategoryRepositoryImpl : CategoryRepository
    {
        private readonly ModelContext _context;

        public CategoryRepositoryImpl(ModelContext context)
        {
            _context = context;
        }



        internal static Category transferToDBCategory(string id, string? type_model)
        {
            //category 表
            var db_add_classification = new Category
            {
                CommodityId = id,
                Type = type_model,
            };

            return db_add_classification;
        }

        internal static List<CommodityProperty> transferToDBProperty(string id, Dictionary<string, List<string>> model_property)
        {
            var db_add_property = new List<CommodityProperty>();
            foreach (var property in model_property)
            {
                foreach (var val in property.Value)
                {
                    var new_property = new CommodityProperty
                    {
                        CommodityId = id,
                        PropertyType = property.Key,
                        PropertyValue = val,
                    };
                    db_add_property.Add(new_property);
                }
            }
            return db_add_property;
        }

        public static List<Pick> transferToDBPick(List<DPick> pick_model)
        {
            var db_add_picks = new List<Pick>();
            foreach (DPick pick in pick_model)
            {
                var new_pick = new Pick
                {
                    PickId = pick.PickId,
                    Description = pick.Description,
                    CommodityId = pick.CommodityId,
                    IsDeleted = pick.IsDeleted,
                    Price = pick.Price,
                    PropertyType = pick.PropertyType,
                    PropertyValue = pick.PropertyValue,
                    Stock=pick.Stock
                };
                db_add_picks.Add(new_pick);
            }
            return db_add_picks;
        }

        public static List<DPick> transferToModelPick(List<Pick> db_pick)
        {
            var db_add_picks = new List<DPick>();
            foreach (Pick pick in db_pick)
            {
                var new_pick = new DPick
                {
                    PickId = pick.PickId,
                    Description = pick.Description,
                    CommodityId = pick.CommodityId,
                    IsDeleted = pick.IsDeleted,
                    Price = pick.Price,
                    PropertyType = pick.PropertyType,
                    PropertyValue = pick.PropertyValue,
                    Stock = pick.Stock,
                };
                db_add_picks.Add(new_pick);
            }
            return db_add_picks;
        }

        internal Dictionary<string, List<string>> transferToDModelProperty(List<CommodityProperty> property)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            foreach (var item in property)
            {
                string key = item.PropertyType; // 使用第一个字母作为键
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key].Add(item.PropertyValue); // 如果键已存在，将值添加到对应的列表中
                }
                else
                {
                    dictionary[key] = new List<string> { item.PropertyValue }; // 如果键不存在，创建一个新的列表并添加值
                }
            }
            return dictionary;

        }

        //generate pick automatically according to property
        //only for aux,should not use
        private static void generatePick_aux(string id, List<KeyValuePair<string, List<string>>> property, int level, ref List<KeyValuePair<string, string>> record, ref List<DPick> pick)
        {
            if (level >= property.Count())
            {
                var guid = Guid.NewGuid().ToString();
                foreach (var records in record)
                {
                    pick.Add(new DPick
                    {
                        CommodityId = id,
                        PickId = guid,
                        PropertyType = records.Key,
                        PropertyValue = records.Value,
                    });
                }
                return;
            }
            var property_value = property[level];

            foreach (var val in property_value.Value)
            {
                record.Add(new KeyValuePair<string, string>(property_value.Key, val));
                generatePick_aux(id, property, level + 1, ref record, ref pick);
                record.RemoveAt(record.Count - 1);
            }
        }

        //for use
        internal static List<DPick> generatePick(string id, Dictionary<string, List<string>> property)
        {
            var pick = new List<DPick>();
            var record = new List<KeyValuePair<string, string>>();
            generatePick_aux(id, property.ToList(), 0, ref record, ref pick);
            return pick;
        }


        public async Task addNoTransaction(CategoryAggregate categoryAggregate)
        {
         
            int exception = 0;
            //no commodityId yet
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == categoryAggregate.ProductId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("you still don't have the commodity");
            try
            {
                exception = 1;
                //property 表
                var db_add_property = transferToDBProperty(categoryAggregate.ProductId, categoryAggregate.Property);
                await _context.CommodityProperties.AddRangeAsync(db_add_property);
                await _context.SaveChangesAsync();

                exception = 2;
                //pick: pick is generated by property using permutation thoughts
                var model_add_picks = generatePick(categoryAggregate.ProductId, categoryAggregate.Property);
                var db_add_picks = transferToDBPick(model_add_picks);
                await _context.Picks.AddRangeAsync(db_add_picks);
                
                await _context.SaveChangesAsync();

                exception = 3;
                //category 表
                var db_add_classification = transferToDBCategory(categoryAggregate.ProductId, categoryAggregate.ClassficationType);
                
                await _context.Categories.AddAsync(db_add_classification);
                await _context.SaveChangesAsync();            
            }

            catch
            {
                switch (exception)
                {
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
        }


        //exception fixed
        public async Task add(CategoryAggregate categoryAggregate)
        {
            //transaction begin
            IDbContextTransaction? tran = null;
            int exception = 0;
            //no commodityId yet
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == categoryAggregate.ProductId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("you still don't have the commodity");
            try
            {
                tran = _context.Database.BeginTransaction();
                exception = 1;
                //property 表
                var db_add_property = transferToDBProperty(categoryAggregate.ProductId, categoryAggregate.Property);
                
                await _context.CommodityProperties.AddRangeAsync(db_add_property);
                await _context.SaveChangesAsync();

                exception = 2;
                //pick: pick is generated by property using permutation thoughts
                var model_add_picks = generatePick(categoryAggregate.ProductId, categoryAggregate.Property);
                var db_add_picks = transferToDBPick(model_add_picks);
                await _context.Picks.AddRangeAsync(db_add_picks);
                
                await _context.SaveChangesAsync();

                exception = 3;
                //category 表
                var db_add_classification = transferToDBCategory(categoryAggregate.ProductId, categoryAggregate.ClassficationType);
                
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
                switch (exception)
                {
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

        //exception fixed
        internal async Task updateProperty(string id, Dictionary<string, List<string>> property)
        {
            try
            {
                var db_ori_property = _context.CommodityProperties.Where(x => x.CommodityId == id).ToList();
                //transfer to dictionary
                var model_ori_property = transferToDModelProperty(db_ori_property);

                //compare the key of the two dictinaries(no order),using hashset

                bool same_key = AreKeysEqual.KeysEqual(model_ori_property, property);

                //update property
                var db_update_property = transferToDBProperty(id, property);
                var remove_old_property = db_ori_property.Except(db_update_property).ToList();
                _context.CommodityProperties.RemoveRange(remove_old_property);
                var add_new_property = db_update_property.Except(db_ori_property).ToList();
                _context.CommodityProperties.AddRange(add_new_property);

                //update pick
                var old_db_pick = _context.Picks.Where(x => x.CommodityId == id).ToList();

                var new_db_pick = transferToDBPick(generatePick(id, property));

                //A:original B:current
                //algorithm: (A-B)in common + (B-A) not in common
                if (same_key)
                {
                    //remove original by using group and function object
                    var old_group = old_db_pick.GroupBy(p => p.PickId);
                    var new_group = new_db_pick.GroupBy(p => p.PickId);
                    var remove_ori = old_group.Except(new_group, new MyCompare.GroupingComparer());
                    var update_new = new_group.Except(old_group, new MyCompare.GroupingComparer());
                    //change IEnumerable<string,T> in List<T> 
                    _context.Picks.RemoveRange(remove_ori.SelectMany(p => p));
                    _context.Picks.AddRange(update_new.SelectMany(p => p));

                }
                //delete all and add new
                else
                {
                    //remove all original
                    _context.Picks.RemoveRange(old_db_pick);
                    //add all new
                    var model_add_picks = generatePick(id, property);
                    var db_add_picks = transferToDBPick(model_add_picks);
                    await _context.Picks.AddRangeAsync(db_add_picks);
                }
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("update property and picks failure");
            }


        }


        



        //exception fixed
        public async Task setPick(PickDto picks)
        {
            //the arguments in filter are like this
            //@arguments        @query
            //"color"           "red"
            //"size"            "big"
            //"made"            "silk"
            var db_picks = getPicksAux(picks);

            foreach (var item in db_picks)
            {
                foreach (var pick in item)
                {
                    //the arguments in change are like this
                    //@arguments        
                    //isDeleted   y/n
                    //price eg:100.02
                    //description
                    //stock
   
                    if (picks.IsDeleted != null)
                        pick.IsDeleted = picks.IsDeleted;


                    pick.Price = picks.Price ?? pick.Price;

                    pick.Description = picks.Description ?? pick.Description;

                    pick.Stock = picks.Stock ?? pick.Stock;

                }
            }



            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("reset pick failure");
            }

           

        }

       
        //exception fixed
        internal async Task updateCategory(string id, string? type)
        {
            try
            {
                var db_update_category = transferToDBCategory(id, type);
                var db_ori_category = _context.Categories.Where(x => x.CommodityId == id).FirstOrDefault();
                //db_ori_category不可能为空
                db_ori_category.Type = db_update_category.Type;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("update category failure");
            }
        }

        public async Task updateNoTransaction(CategoryAggregate categoryAggregate)
        {
            //如果根本没有这个commodityId
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == categoryAggregate.ProductId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("you still don't have the commodity");
  
            int exception = 0;
            try
            {
                
                //property and pick
                await updateProperty(categoryAggregate.ProductId, categoryAggregate.Property);
                exception = 1;

                //category
                await updateCategory(categoryAggregate.ProductId, categoryAggregate.ClassficationType);
                exception = 2;
                
            }
            catch
            {
                
                switch (exception)
                {

                    case 0:
                        throw new DuplicateException("same property or pick occurred");
                    
                    case 1:
                        throw new DuplicateException("category already exists");
                  
                }
            }
            

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
                //property and pick
                await updateProperty(categoryAggregate.ProductId, categoryAggregate.Property);
                exception = 1;

                //category
                await updateCategory(categoryAggregate.ProductId, categoryAggregate.ClassficationType);
                exception = 2;
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                switch (exception)
                {

                    case 0:
                        throw new DuplicateException("same property or pick occurred");
                    
                    case 1:
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

        //exception fixed
        public CategoryAggregate getById(string commodityId)
        {
            var existCommodity = _context.CommodityGenerals.Where(x => x.CommodityId == commodityId).FirstOrDefault();
            if (existCommodity == null)
                throw new NotFoundException("The commodity does not exist");

            var db_property = _context.CommodityProperties.Where(x => x.CommodityId == commodityId).ToList();

            var domain_property = transferToDModelProperty(db_property);

            var db_category = _context.Categories.Where(x => x.CommodityId == commodityId).FirstOrDefault();

            string? domain_category = (db_category == null ? null :db_category.Type);

            var db_pick = _context.Picks.Where(x => x.CommodityId == commodityId);

            var domain_pick = new List<DPick>();
            foreach (var it in db_pick)
            {
                domain_pick.Add(new DPick
                {
                    CommodityId = it.CommodityId,
                    Description = it.Description,
                    IsDeleted = it.IsDeleted,
                    PickId = it.PickId,
                    Price = it.Price,
                    PropertyType = it.PropertyType,
                    PropertyValue = it.PropertyValue,
                    Stock=it.Stock
                    
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


        //exception fixed
        public async Task delete(string commodityId)
        {
            try
            {
                //property 表
                var db_add_property = _context.CommodityProperties.Where(x => x.CommodityId == commodityId);

                _context.CommodityProperties.RemoveRange(db_add_property);
                //pick表将会级联删除，无须操作

                //category 表
                var db_add_classification = _context.Categories.Where(x => x.CommodityId == commodityId);

                _context.Categories.RemoveRange(db_add_classification);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("delete failure");
            }

        }


        internal List<IGrouping<string, Pick>> getPicksAux(PickDto picks)
        {

            var id = picks.commodityId;
            var filter = picks.Filter;
            var pickId = picks.PickId;
            var db_picks = _context.Picks;
            if (pickId==null)
            {
                var db_picks_group= db_picks.Where(p => p.CommodityId == id).GroupBy(p => p.PickId).ToList();
                
                foreach (var item in filter)
                {
                    db_picks_group = db_picks_group.Where(g => g.Any(pv => pv.PropertyType == item.Key && pv.PropertyValue == item.Value)).ToList();
                }
                return db_picks_group;
            }
            else
            {
                return db_picks.Where(p => p.PickId == pickId).GroupBy(p => p.PickId).ToList();
            }
        
           
        }

        public List<IGrouping<string, DPick>> getPicks(PickDto picks)
        {
            var id = picks.commodityId;
            var filter = picks.Filter;
            var pickId = picks.PickId;
            var db_picks = transferToModelPick(_context.Picks.ToList());

            if (pickId == null)
            {
                var db_picks_group = db_picks.Where(p => p.CommodityId == id).GroupBy(p => p.PickId).ToList();

                foreach (var item in filter)
                {
                    db_picks_group = db_picks_group.Where(g => g.Any(pv => pv.PropertyType == item.Key && pv.PropertyValue == item.Value)).ToList();
                }
                return db_picks_group;
            }
            else
            {
                return db_picks.Where(p => p.PickId == pickId).GroupBy(p => p.PickId).ToList();
            }
        }




      









    }
}
