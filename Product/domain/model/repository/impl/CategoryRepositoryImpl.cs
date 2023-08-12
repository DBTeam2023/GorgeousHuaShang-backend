using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Product.common;
using Product.exception;
using Product.utils;
using Product.dto;
namespace Product.domain.model.repository.impl
{
    public class CategoryRepositoryImpl
    {
        private readonly ModelContext _context;

        public CategoryRepositoryImpl(ModelContext context)
        {
            _context = context;
        }

        
        
        internal static Category transferToDBCategory(string id,BasicSortType? type_model)
        {
            //category 表
            var db_add_classification = new Category
            {
                CommodotyId = id,
                Type = type_model == null?null:BasicSortType.getAns(type_model),
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

        internal static List<Pick> transferToDBPick(List<DPick> pick_model)
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
                };
            }
            return db_add_picks;
        }

        internal Dictionary<string,List<string>> transferToDModelProperty(List<CommodityProperty> property)
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

                //property 表
                var db_add_property = transferToDBProperty(categoryAggregate.ProductId,categoryAggregate.Property);
                exception = 1;   
                await _context.CommodityProperties.AddRangeAsync(db_add_property);
                await _context.SaveChangesAsync();

                //pick: pick is generated by property using permutation thoughts
                var model_add_picks = generatePick(categoryAggregate.ProductId,categoryAggregate.Property);
                var db_add_picks = transferToDBPick(model_add_picks);
                await _context.Picks.AddRangeAsync(db_add_picks);
                exception = 2;
                await _context.SaveChangesAsync();


                //category 表
                var db_add_classification = transferToDBCategory(categoryAggregate.ProductId,categoryAggregate.ClassficationType);
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
        internal async Task updateProperty(string id,Dictionary<string, List<string>> property)
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
        internal async Task updatePick(string id,List<DPick> pick)
        {
            try
            {
                var db_update_pick = transferToDBPick(pick);
                var db_ori_pick = _context.Picks.Where(x => x.CommodityId == id).ToList();
                var remove_old_pick = db_ori_pick.Except(db_update_pick).ToList();
                _context.Picks.RemoveRange(remove_old_pick);
                var add_new_pick = db_update_pick.Except(db_ori_pick);
                _context.Picks.AddRange(add_new_pick);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("update pick failure");
            }
            
        }

        //exception fixed
        internal async Task updateCategory(string id,BasicSortType? type)
        {
            try
            {
                var db_update_category = transferToDBCategory(id, type);
                var db_ori_category = _context.Categories.Where(x => x.CommodotyId == id).FirstOrDefault();
                //db_ori_category不可能为空
                db_ori_category.Type = db_update_category.Type;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("update category failure");
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
                        throw new DuplicateException("same property occurred");
                    case 1:
                        throw new DuplicateException("same pick occurred");
                    case 2:
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


        //exception fixed
        public async Task delete(string commodityId)
        {
            var categoryAggregate = getById(commodityId);

            try
            {
                //property 表
                var db_add_property = transferToDBProperty(categoryAggregate.ProductId,categoryAggregate.Property);

                _context.CommodityProperties.RemoveRange(db_add_property);
                //pick表将会级联删除，无须操作

                //category 表
                var db_add_classification = transferToDBCategory(categoryAggregate.ProductId,categoryAggregate.ClassficationType);

                _context.Categories.Remove(db_add_classification);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("delete failure");
            }
            
        }



        ////not used
        //public IPage<CategoryAggregate> pageQuery(PageQueryDto pageQuery)
        //{
        //    //the arguments are like this
        //    //@arguments        @query
        //    //"color"           "red"
        //    //"size"            "big"
        //    //"made"            "silk"
        //    var db_picks = _context.Picks.GroupBy(p => p.PickId);
        //    if(pageQuery.Filter==null)
        //    {
        //        ;
        //    }
        //    else
        //    {

        //        db_picks = db_picks.Where(p =>
        //        p.Any(pick => pageQuery.Filter.Any(item =>
        //        pick.PropertyType == item.Key && pick.PropertyValue == item.Value)));

        //    }

        //}








        //should be written in ProductRepository and ProductRepositoryImpl
        public IPage<ProductAggregate> pageQuery2(PageQueryDto pageQuery)
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
            var all = _context.CommodityGenerals;

            var ans = _context.CommodityGenerals
                .Where(x => x.CommodityId == (pageQuery.getStrValue("commodityId")??x.CommodityId))
                .Where(x => x.StoreId == (pageQuery.getStrValue("storeId")??x.StoreId))
                .Where(x => double.Parse(x.Price) >= (pageQuery.getDoubleValue("pricemin")??int.MinValue))
                .Where(x => double.Parse(x.Price) <= (pageQuery.getDoubleValue("pricemax")??int.MaxValue))
                .Where(x => x.CommodityName.Contains((pageQuery.getStrValue("name")??"")))
                .Where(x => x.Description!=null&&x.Description.Contains((pageQuery.getStrValue("description")??"")));

            var ans_type=_context.Categories.
                Where(x => x.Type != null && x.Type.Contains((pageQuery.getStrValue("type") ?? "")));

            var result = ans.Join(ans_type,
                 ansItem => ansItem.CommodityId,
                 ansTypeItem => ansTypeItem.CommodotyId,
                 (ansItem, ansTypeItem) => ansItem).ToList();

            int page_index = pageQuery.PageIndex;
            int page_size = pageQuery.PageSize;

            if(result.Count()/page_size+1<page_index)
            {
                result = result.GetRange((page_index - 1) * page_size - 1, page_size);
            }
            else if(result.Count() / page_size + 1 > page_index)
            {
                result.Clear();//an empty list
            }
            else
            {
                result = result.GetRange((page_index - 1) * page_size - 1, result.Count()- (page_index - 1) * page_size + 1);
            }




            var page = IPage<ProductAggregate>.builder()
                .size(page_size).current(page_index).records().build();

            return page;

        }





    }
}
