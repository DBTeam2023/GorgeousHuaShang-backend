using EntityFramework.Models;

namespace Logistics.core.vo
{
    public class LogisticVo
    {
        public string LogisticsId { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Company { get; set; } = null!;

        public string ShipAddress { get; set; } = null!;

        public string PickAddress { get; set; } = null!;


        public LogisticVo(Logistic x)
        {
            LogisticsId = x.LogisticsId;
            StartTime = x.StartTime;
            EndTime = x.EndTime;
            Company = x.Company;
            ShipAddress = x.ShipAddress;
            PickAddress = x.PickAddress;
        }

    }
}
