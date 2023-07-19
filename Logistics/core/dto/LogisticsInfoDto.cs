namespace Logistics.core.dto
{
    public class LogisticsInfoDto
    {
        public string LogisticsId { get; set; } = null!;

        public string ArrivePlace { get; set; } = null!;

        public DateTime ArriveTime { get; set; }
    }
}
