using Order.domain.model;
using Order.dto;

namespace Order.domain.service
{
    public interface OrderService
    {
        public Task<string> ChangeOrderCancel(string token, string orderID);
        public Task ChangeOrderPaidSuccess(string token, string orderID);

        public Task ChangeOrderPaidComplete(string token, string orderID);


    }
}
