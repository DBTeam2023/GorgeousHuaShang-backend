namespace Logistics.core.dto
{
    public class LogisticDto
    {
        //public string LogisticsId { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public string Company { get; set; } = null!;

        public string ShipAddress { get; set; } = null!;

        public string PickAddress { get; set; } = null!;

    }
}
