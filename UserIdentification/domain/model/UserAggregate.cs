using System.Text.Json.Serialization;
using UserIdentification.common;
using UserIdentification.domain.model.repository;
using UserIdentification.domain.model.repository.impl;

namespace UserIdentification.domain.model
{
    public class UserAggregate
    {
        //autowired
        static UserRepository userRepository = new UserRepositoryImpl(new dataaccess.mapper.ModelContext());

        public string UserId { get; set; } = null!;

        public DateTime? LoginTime { get; set; }

        public string Password { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Type { get; set; } = null!;

        public BuyerEntity? buyerInfo { get; set; }

        public SellerEntity? sellerInfo { get; set; }

        public UserAggregate(UserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        internal UserAggregate() { }

        internal UserAggregate(string username, string password, string type)
        {
            Guid id = Guid.NewGuid();
            UserId = id.ToString();
            Username = username;
            Password = password;
            Type = type;
        }

        internal UserAggregate(string userId, string username, string password, string type)
        {
            UserId = userId;
            Password = password;
            Username = username;
            Type = type;
        }

        internal UserAggregate(string userId, string nickName, string phonenumber, string username, string password, string type)
        {
            UserId = userId;
            Password = password;
            NickName = nickName;
            PhoneNumber = phonenumber;
            Username = username;
            Type = type;
        }

        [JsonConstructor]
        public UserAggregate(string userId, DateTime? loginTime, string password, string nickName, string username, string phonenumber, string type, BuyerEntity? buyerInfo, SellerEntity? sellerInfo)
        {
            UserId = userId;
            LoginTime = loginTime;
            Password = password;
            NickName = nickName;
            Username = username;
            PhoneNumber = phonenumber;
            Type = type;
            this.buyerInfo = buyerInfo;
            this.sellerInfo = sellerInfo;
        }

        public static UserAggregate create(string username, string password, string type)
        {
            //security check
            UserType.TypeCheck(type);

            //initialization
            Guid id = Guid.NewGuid();
            UserAggregate newUser = new UserAggregate(id.ToString(),username, password, type);
            //newUser.UserId = id.ToString();
            if(type == UserType.Buyer)
            {
                newUser.buyerInfo = new BuyerEntity()
                {
                    UserId = id.ToString(),
                    IsVip = false
                };
            }
            else if(type == UserType.Seller)
            {
                newUser.sellerInfo = new SellerEntity()
                {
                    UserId = id.ToString()
                };
            }
            else { }  //administrator

            //data access
            userRepository.add(newUser);

            return newUser;
        }
    }
}
