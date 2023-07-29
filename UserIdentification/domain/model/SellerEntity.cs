namespace UserIdentification.domain.model
{
    public class SellerEntity
    {
        public string UserId { get; set; } = null!;

        public string? Address { get; set; }

        public SellerEntity() { }

        protected static SellerEntity create(string userId, string address)
        {

            return null;
        }
    }
}
