using Microsoft.AspNetCore.Mvc;
using UserIdentification.exception;

namespace UserIdentification.utils
{
    /**
     * @author sty
     * @brief image access
     * 
     * @param directory path
     * @param image type
     */
    public class ImageService
    {
        public string directoryPath;
        public string imageType = "png";

        public ImageService(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public ImageService(string directoryPath, string imageType)
        {
            this.directoryPath = directoryPath;
            this.imageType = imageType;
        }

        public void setImage(IFormFile image, string imageName)
        {
            if (image == null)
            {
                throw new InvalidTypeException("null image");
            }

            string fileExtension = Path.GetExtension(image.FileName);
            if (fileExtension != "." + imageType)
            {
                throw new InvalidTypeException("invalid file extension");
            }
            string filename = imageName + fileExtension;
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

        public FileContentResult getImage(string imageName)
        {
            string filename = imageName + "." + imageType;
            string imagePath = Path.Combine(directoryPath, filename); // file path

            try
            {
                byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
                string contentType = "image/" + imageType; // file type

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
