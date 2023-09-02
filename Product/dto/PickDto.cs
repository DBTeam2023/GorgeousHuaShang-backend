using Product.exception;
using Product.utils;
using System.Text.Json.Serialization;

namespace Product.dto
{
    public class PickDto
    {

        public string? PickId { get; set; }

        public string? commodityId { get; set; }
        public Dictionary<string, string>? Filter { get; set; }
       
        public bool? IsDeleted { get; set; }
        
        public decimal? Price { get; set; }
        
        public string? Description { get; set; }

        public decimal? Stock { get; set; }

        public IFormFile? image { get; set; }


        public PickDto(PickAuxDto pick)
        {
            commodityId = pick.commodityId;
            if(pick.Filter!=null)
                Filter = JsonConvertService<Dictionary<string, string>>.convertToJson(pick.Filter);
            IsDeleted = pick.IsDeleted;
            Price = pick.Price;
            Description = pick.Description;
            Stock = pick.Stock;
            image = pick.image;
            PickId = pick.PickId;
            check();
        }

        private void check()
        {
            if (PickId != null && commodityId == null && Filter == null)
                ;
            else if (PickId == null && commodityId != null)
                ;
            else
                throw new InvalidTypeException("not comformance");

        }

       
    }
}
