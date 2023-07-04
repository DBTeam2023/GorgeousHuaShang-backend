using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UserIdentification.entity
{
    public class User
    {
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(18)]
        [Display(Name = "身份证号")]
        public long IdCard { get; set; }

        //[DataType(DataType.Date)]
        //[Display(Name = "出生日期")]
        //public DateTime BirthDate { get; set; }

        [Display(Name = "家庭住址")]
        public string Address { get; set; }

        public User(string username, string password, long idCard, string address)
        {
            Username = username;
            Password = password;
            IdCard = idCard;
            Address = address;
        }
    }
}
