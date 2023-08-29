namespace Storesys.core.dto
{
    public class StoreGetPageDto
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }

        public string? storeName { get; set; } = null!;
        public string? token { get; set; } = null!;
    }
}
