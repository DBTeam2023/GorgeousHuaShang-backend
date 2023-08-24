using Microsoft.AspNetCore.Mvc;
using UserIdentification.exception;

namespace UserIdentification.domain.service.impl
{
    public class AvatarServiceImpl : AvatarService
    {
        public AvatarServiceImpl() { }

        public string directoryPath = "C:\\Users\\Administrator\\Desktop\\image\\avatar";
        //public string directoryPath = "C:\\Users\\admin\\Desktop\\DBPrinciple\\FinalProject\\testImage";

        public string? Address { get; set; }

        public void setAvatar(IFormFile image, string avatarName)
        {
            if(image == null)
            {
                throw new InvalidTypeException("null image");
            }

            string filename = avatarName + Path.GetExtension(image.FileName);
            string imagePath = Path.Combine(directoryPath, filename);
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.OpenOrCreate))
                {
                    image.CopyTo(stream);  // save the picture
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException("directory not found");
            }
            
            return;
        }

        public FileContentResult getAvatar(string avatarName)
        {
            string filename = avatarName + ".png";
            string imagePath = Path.Combine(directoryPath, filename); // file path

            try
            {
                byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
                string contentType = "image/png"; // file type

                return new FileContentResult(imageData, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new NotFoundException("image file not found");
            }
        }
    }
}
