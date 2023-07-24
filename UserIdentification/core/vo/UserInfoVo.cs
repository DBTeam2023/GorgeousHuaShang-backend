using EntityFramework.Models;

namespace UserIdentification.core.vo
{
    public class UserInfoVo
    {
        public string Username { get; set; } = null!;

        public string Type { get; set; } = null!;

        public UserInfoVo(User user)
        {
            Username = user.NickName; Type = user.Type;
        }
    }
}
