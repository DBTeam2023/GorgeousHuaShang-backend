using Microsoft.AspNetCore.Mvc;
using UserIdentification.exception;
using UserIdentification.utils;

namespace UserIdentification.domain.service.impl
{
    public class AvatarServiceImpl : AvatarService
    {
        public static string directoryPath = "C:\\Users\\Administrator\\Desktop\\image\\avatar";

        ImageService imageService = new ImageService(directoryPath);

        public AvatarServiceImpl() { }

        public void setAvatar(IFormFile image, string avatarName)
        {
            imageService.setImage(image, avatarName);
        }

        public FileContentResult getAvatar(string avatarName)
        {
            return imageService.getImage(avatarName);
        }
    }
}
