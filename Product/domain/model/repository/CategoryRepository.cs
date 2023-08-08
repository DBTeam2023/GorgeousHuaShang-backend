namespace Product.domain.model.repository
{
    public interface CategoryRepository
    {
        public Task add(CategoryAggregate categoryAggregate);

        public Task update(CategoryAggregate categoryAggregate);


        public CategoryAggregate getById(string commodityId);

        //全部删除 classification property pick
        public Task delete(string commodityId);

        
        
    }
}
