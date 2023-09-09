using Product.domain.model;

namespace Product.dto
{
    public class PickGroupDto
    {
        public List<IGrouping<string, DPick>> pickList { get; set; } = null!;

        public string? storeId { get; set; }
        public string? productName { get; set; }
    }
}
