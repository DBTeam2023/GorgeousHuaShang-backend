using System.Text.Json;
using Product.exception;
namespace Product.utils
{
    public class JsonConvertService<T>
    {
        public static T convertToJson(string str)
        {           
            try
            {
                var dict = JsonSerializer.Deserialize<T>(str);
                return dict;
            }
            catch
            {
                throw new ConvertException("json convert failure.Invalid string expression");
            }

            
        }



    }
}
