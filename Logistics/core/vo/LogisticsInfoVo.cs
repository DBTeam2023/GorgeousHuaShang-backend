using EntityFramework.Models;
namespace Logistics.core.vo
{
    public class LogisticsInfoVo
    {
        public string ArrivePlace { get; set; } = null!;

        public DateTime ArriveTime { get; set; }

        public LogisticsInfoVo(Logisticsinfo x)
        {
            ArrivePlace = x.ArrivePlace;
            ArriveTime = x.ArriveTime;
        }
    }
}
