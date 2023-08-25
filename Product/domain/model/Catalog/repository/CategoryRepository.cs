using EntityFramework.Models;
using Product.dto;

namespace Product.domain.model.repository
{
    public interface CategoryRepository
    {
        public Task add(CategoryAggregate categoryAggregate);

        public Task addNoTransaction(CategoryAggregate categoryAggregate);

        public Task update(CategoryAggregate categoryAggregate);

        public Task updateNoTransaction(CategoryAggregate categoryAggregate);

        public CategoryAggregate getById(string commodityId);

        //全部删除 classification property pick
        public Task delete(string commodityId);

        public Task setPick(PickDto picks);

        public List<IGrouping<string, Pick>> getPicks(PickDto picks);


    }
}
