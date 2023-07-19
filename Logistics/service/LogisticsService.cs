using EntityFramework.Models;
namespace Logistics.service
{
    public interface LogisticsService
    {
       

        void setTimespan(double value);
        public TimeSpan getTimespan();

       

        //物流商品于某时到达某地（添加信息）
        public Task<Logisticsinfo> addLogisticsInfo(string id, string place, DateTime time);

        
        

        //获得所有物流信息
        public IList<Logisticsinfo> getAllLogisticsInfo(string id);

        //物流是否到达终点
        public bool arriveDestination(string id);

        //下面与logistics有关
        public Task<Logistic> addLogistics(DateTime start, string company, string address_beg, string address_end);
       
        public Task deleteLogistics();

        public Logistic getLogistics(string id);

        public Task<Logistic> addArrivalTime(string id, DateTime end);

        public Task openRegularClear();

        public void endRegularClear();


    }
}
