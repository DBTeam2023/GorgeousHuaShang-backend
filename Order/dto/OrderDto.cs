namespace Order.dto
{
    public class OrderDto
    {
        public string OrderID { get; set; } = null!;

        public DateTime CreateTime { get; set; } 

        public decimal Money { get; set; }

        public bool State { get; set; }

        public string LogisticID { get; set; } = null!;
        public string[] PickID { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

    }
}
