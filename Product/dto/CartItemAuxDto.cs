namespace Product.dto
{
    public class CartItemAuxDto
    {
        public string PickId { get; set; } = null!;

       // public int Number { get; set; }

        public IFormFile? Image { get; set; }

        public bool? IsDeleted { get; set; }
        //public decimal? Price { get; set; }

        //public decimal? TotalPrice { get; set; }

        public CartItemAuxDto(string pickId,bool? isDeleted,IFormFile?image)
        {
            PickId = pickId;
            //Number = number;
            IsDeleted = isDeleted;
            Image = image;
        }
    }
}
