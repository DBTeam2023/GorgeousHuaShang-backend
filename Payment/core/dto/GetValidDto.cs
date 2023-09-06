using EntityFramework.Models;
namespace Payment.core.dto
{
    public class GetValidDto
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        // public string token { get; set; }
        public List<string> pickIds { get; set; }
    }
}
