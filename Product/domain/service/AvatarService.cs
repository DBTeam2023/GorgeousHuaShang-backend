using Microsoft.AspNetCore.Mvc;

namespace Product.domain.service
{
    public interface AvatarService
    {
        public void setCommodityAvatar(IFormFile? image, string avatarName);

        public void setPickAvatar(IFormFile? image, string avatarName);


        public FileContentResult getCommodityAvatar(string avatarName);


        public FileContentResult getPickAvatar(string avatarName, string upperAvatarName);

        public void deleteCommodityAvatar(string avatarName);

        public void deletePickAvatar(string avatarName);


    }
}
