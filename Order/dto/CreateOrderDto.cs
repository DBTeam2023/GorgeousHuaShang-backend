namespace Order.dto
{
    public class CreateOrderDto
    {
        public class CreateOrder
        {
            public string PickId { get; set; } = null!;

            public decimal Number { get; set; }
        }

        public List<CreateOrder> OrderCreate { get; set; } = null!;      
        

    }
}
