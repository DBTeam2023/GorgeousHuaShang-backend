using Product.dto;
using Product.utils;

namespace Product.domain.model.repository
{
    public interface ProductRepository
    {
        public Task add(ProductAggregate productAggregate);

        public Task update(ProductAggregate productAggregate);

        public ProductAggregate getById(string commodityId);

        public Task delete(string commodityId);

        public IPage<ProductAggregate> pageQuery(PageQueryDto pageQuery);
    }
}
