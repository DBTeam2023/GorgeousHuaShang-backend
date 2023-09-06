namespace UserIdentification.domain.model
{
    public class BuyerEntity
    {
        public string UserId { get; set; } = null!;

        public string? Address { get; set; }

        public byte? Age { get; set; }

        public bool? Gender { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public bool IsVip { get; set; }
    }
}
