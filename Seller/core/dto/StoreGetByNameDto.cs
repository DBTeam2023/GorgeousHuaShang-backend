namespace Storesys.core.dto
{
    public class StoreGetByNameDto
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }

        public string? storeName { get; set; } = null!;
    }
}
