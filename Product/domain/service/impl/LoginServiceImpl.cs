//using Product.common;
//using Product.dataaccess.DBModels;
//using Product.dataaccess.mapper;
//using Product.domain.model;
//using Product.domain.model.repository;
//using Product.dto;
//using Product.exception;
//using Product.utils;

//namespace Product.domain.service.impl
//{
//    public class LoginServiceImpl : LoginService
//    {
//        //autowired
//        JwtHelper jwt;
//        ModelContext modelContext;

//        UserRepository userRepository;

//        public LoginServiceImpl(JwtHelper _jwt, ModelContext dbcontext, UserRepository _userRepository)
//        {
//            jwt = _jwt;
//            modelContext = dbcontext;
//            userRepository = _userRepository;
//        }

//        public TokenDto login(string username, string password)
//        {
//            UserAggregate user = userRepository.getByUsername(username);

//            if (user == null)
//            {
//                LoginException exception = new LoginException("user not found");
//                exception.UserNotFound = true;
//                throw exception;
//            }
//            if (user.Password != password)
//            {
//                LoginException exception = new LoginException("password error");
//                exception.PasswordError = true;
//                throw exception;
//            }

//            return new TokenDto(jwt.CreateToken(username));
//        }

//        public TokenDto register(string username, string password, string type)
//        {
//            UserAggregate newUser = UserAggregate.create(username, password, type);

//            return new TokenDto(jwt.CreateToken(newUser.NickName));
//        }

//        public TokenDto resetPassword(string username, string password)
//        {
//            return null;
//        }
//    }
//}
