using System.Text.Json.Serialization;

namespace Product.dto
{
    public class MyFilterDto
    {
        public Dictionary<string, string>? Filter { get; set; }

        [JsonConstructor]
        public MyFilterDto(Dictionary<string, string?> filter)
        {
            Filter = filter;
        }


        public string? getStrValue(string key)
        {
            if (Filter == null)
                return null;

            if (Filter.ContainsKey(key))
                return Filter[key];
            else
                return null;
        }
        public decimal? getDoubleValue(string key)
        {
            var val = getStrValue(key);
            if (val == null)
                return null;
            else
            {
                decimal ans;
                if (decimal.TryParse(val, out ans))
                    return ans;
                else
                    return null;
            }


        }
    }
}
