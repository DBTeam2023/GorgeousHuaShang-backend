namespace Storesys.core.dto
{
    public class CollectionGetDto
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public string? storeName { get; set; } = null!;
     
    }
}
