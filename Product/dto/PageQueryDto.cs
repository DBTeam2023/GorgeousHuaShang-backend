using Product.exception;
namespace Product.dto
{
    public class PageQueryDto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public Dictionary<string, string>? Filter { get; set; }

        public PageQueryDto(int size,int index, Dictionary<string, string>? filter)
        {
            if (size <= 0)
                throw new PageException("page size should be positive");
            if (index <= 0)
                throw new PageException("page index should be larger than 0");

            PageSize = size;
            PageIndex = index;
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
        public double? getDoubleValue(string key)
        {
            var val = getStrValue(key);
            if (val == null)
                return null;
            else
            {
                double ans;
                if (double.TryParse(val, out ans))
                    return ans;
                else
                    return null;
            }
                

        }



    }
}
