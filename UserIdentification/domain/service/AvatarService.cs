using Microsoft.AspNetCore.Mvc;

namespace UserIdentification.domain.service
{
    public interface AvatarService
    {
        public void setAvatar(IFormFile image, string avatarName);

        public FileContentResult getAvatar(string avatarName);
    }
}
