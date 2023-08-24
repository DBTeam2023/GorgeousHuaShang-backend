using Microsoft.AspNetCore.Mvc;
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
            return loginService.register(username, password, type);
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
