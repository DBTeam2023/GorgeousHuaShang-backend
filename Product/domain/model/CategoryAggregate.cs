using System.Text.Json.Serialization;
using Product.common;
namespace Product.domain.model
{
    public class CategoryAggregate
    {
        public string ProductId { get; set; } = null!;

        public List<DPick> DetailPicks;

        
        public Dictionary<string, List<string>> Property;

        public BasicSortType? ClassficationType { get; set; }
        internal CategoryAggregate() { }


        internal CategoryAggregate(string productId,
            List<DPick> picks,
            Dictionary<string, List<string>> property)
        {
            ProductId = productId;
            DetailPicks = picks;
            Property = property;

        }


        [JsonConstructor]
        internal CategoryAggregate(string productId, List<DPick> picks,
            Dictionary<string, List<string>> property,
            BasicSortType? classficationType)
        {
            ProductId = productId;
            DetailPicks = picks;
            Property = property;
            ClassficationType = classficationType;
        }


       


    }
}
