using Microsoft.AspNetCore.Mvc;
using Payment.message;
using UserIdentification.domain.message;
using UserIdentification.domain.model;
using UserIdentification.domain.model.repository;
using UserIdentification.domain.service;
using UserIdentification.dto;

namespace UserIdentification.application.impl
{
    public class UserIdentificationServiceImpl : UserIdentificationService
    {
        UserRepository userRepository;
        LoginService loginService;
        AvatarService avatarService;

        public UserIdentificationServiceImpl(UserRepository _userRepository, LoginService _loginService, AvatarService _avatarService)
        {
            userRepository = _userRepository;
            loginService = _loginService;
            avatarService = _avatarService;
        }

        public TokenDto login(string username, string password)
        {
            return loginService.login(username, password);
        }

        public TokenDto register(string username, string password, string type)
        {
            TokenDto token = loginService.register(username, password, type);

            // configurations
            string ip = "47.115.231.142";
            //string ip = "localhost";
            string user = "admin";
            string pw = "123";

            // send message
            var eventSender = new RabbitMQEventSender(ip, "my_queue", user, pw);
            string userId = userRepository.getByUsername(username).UserId;
            var message = new UserRegistrationMessage(userId);
            eventSender.SendEvent(message);
            //var messageReceiver = new RabbitMQMessageReceiver("localhost", "my_queue");
            //messageReceiver.StartReceiving();
            //Console.WriteLine(messageReceiver);

            return token;
        }

        public UserAggregate getUserInfoByUsername(string username)
        {
            return userRepository.getByUsername(username);
        }

        public void update(UserAggregate userAggregate)
        {
            userRepository.update(userAggregate);
        }

        public void setAvatar(IFormFile image, string avatarName)
        {
            avatarService.setAvatar(image, avatarName);
            return;
        }

        public FileContentResult getAvatar(string avatarName)
        {
            return avatarService.getAvatar(avatarName);
        }
    }
}
