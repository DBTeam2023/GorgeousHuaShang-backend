namespace Order.dto
{
    public class CreateOrderDto
    {

        public DateTime CreateTime { get; set; }

        public decimal Money { get; set; }

        public bool State { get; set; }

        public string LogisticID { get; set; } = null!;
        public string[] PickID { get; set; } = null!;
        public string UserID { get; set; } = null!;

    }
}
