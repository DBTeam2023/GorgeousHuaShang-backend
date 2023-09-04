namespace Order.dto
{
    public class PickInfoDto
    {
       
        public string PickID { get; set; } = null!;
        public string StoreID { get; set; } = null!;
        public Dictionary<string, string> Property { get; set; } = null!;
        public string CommodityID { set; get; } = null!;

    }
}
