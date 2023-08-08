//using Product.domain.model;
//using Product.domain.model.repository;
//using Product.domain.service;
//using Product.dto;

//namespace Product.application.impl
//{
//    public class ProductServiceImpl : ProductService
//    {
//        UserRepository userRepository;
//        LoginService loginService;

//        public ProductServiceImpl(UserRepository _userRepository, LoginService _loginService)
//        {
//            userRepository = _userRepository;
//            loginService = _loginService;
//        }

//        public TokenDto login(string username, string password)
//        {
//            return loginService.login(username, password);
//        }

//        public TokenDto register(string username, string password, string type)
//        {
//            return loginService.register(username, password, type);
//        }

//        public UserAggregate getUserInfoByUsername(string username)
//        {
//            return userRepository.getByUsername(username);
//        }

//        public void update(UserAggregate userAggregate)
//        {
//            userRepository.update(userAggregate);
//        }
//    }
//}
