namespace Order.dto
{
    public class CreateOrderDto
    {

        public string CreateTime { get; set; } = null!;

        public decimal Money { get; set; }

        public int State { get; set; }

        public string LogisticID { get; set; } = null!;
        public string[] PickID { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

    }
}
