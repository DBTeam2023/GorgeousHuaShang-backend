using EntityFramework.Models;
namespace Storesys.core.vo
{
    public class SellerStoreVo
    {
        public string userId { get; set; }
        public string storeId { get; set; }
        public decimal? isManager { get; set; }

        public SellerStoreVo(SellerStore x)
        {
            userId = x.UserId;
            storeId = x.StoreId;
            isManager = x.Ismanager;
        }
    }
}
