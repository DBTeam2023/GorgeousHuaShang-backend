using System.Text.Json.Serialization;
using Product.common;
namespace Product.domain.model
{
    public class CategoryAggregate
    {
        public string ProductId { get; set; } = null!;

        public List<DPick> DetailPicks { get; set; }
        
        public Dictionary<string, List<string>> Property { get; set; }

        public string? ClassficationType { get; set; }

       

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
            string? classficationType)
        {
            ProductId = productId;
            DetailPicks = picks;
            Property = property;
            ClassficationType = classficationType;
        }


       


    }
}
