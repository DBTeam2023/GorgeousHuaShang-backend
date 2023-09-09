namespace Storesys.core.dto
{
    public class StoreAddDto
    {
        public string storeName { get; set; }
        public int isManager { get; set; }
        public string? des { get; set; } = null!;
        public string? address { get; set; } = null!;
        public IFormFile? image { get; set; } = null;
    }
}
