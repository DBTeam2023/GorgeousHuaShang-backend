using UserIdentification.dataaccess.DBModels;

namespace UserIdentification.resource.vo
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
