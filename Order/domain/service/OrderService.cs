using Order.domain.model;
using Order.dto;

namespace Order.domain.service
{
    public interface OrderService
    {

        public BuyerInfoDto getBuyerInfo(string userID);

        public Task<PickInfoDto[]> getPickInfos(string[] pickID);


    }
}
