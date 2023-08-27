using EntityFramework.Models;
namespace Storesys.core.vo
{
    public class StoreVo
    {
        public string storeId { get; set; }
        public string storeName { get; set; }   
        public decimal score { get; set; }
        public bool isDeleted { get; set; }

        public StoreVo(Store x)
        {
            storeId = x.StoreId;
            storeName = x.StoreName;
            score = x.Score;
            isDeleted = x.IsDeleted;
        }
    }
}
