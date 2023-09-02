
using Microsoft.AspNetCore.Mvc;
using Product.exception;

namespace Product.utils
{
    /**
     * @author szc
     * @baseed on sty
     * @brief image access
     * 
     * @param directory path
     * @param image type
     */
    public class ImageService
    {
        public string directoryPath;

        public static string[] suffix = { ".png", ".jpg", ".jpeg" };

        public static readonly int RestrictedKb = 4;

        public ImageService(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        private void suffixCheck(string mySuffix)
        {
            foreach(var str in suffix)
            {
                if (mySuffix == str)
                    return;
            }
            throw new InvalidTypeException("invalid file extension");
        }


      
        public string GetImageExtension(IFormFile file)
        {
            string contentType = file.ContentType;
            string extension = Path.GetExtension(file.FileName);
            suffixCheck(extension);
            return extension;
        }

        //exception fixed
        public void setImage(IFormFile? image, string imageName)
        {

            //delete original
            deleteImage(imageName);
            if (image == null)          
                return;

            string fileExtension = GetImageExtension(image);

            string filename = imageName + fileExtension;

            string imagePath = Path.Combine(directoryPath, filename);

            var imageCompression = new ImageCompressionService(image,imagePath);

            imageCompression.ProcessImage(RestrictedKb);

           

        }

        private string? searchFile(string imageName)
        {
            string searchPattern = imageName + "." + "*";
            string[] files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            if (files.Count() > 1)
                throw new DuplicateException("same image occurred");
            if (files.Count() == 0)
                return null;
            return files[0];
        }



        //exception fixed
        public FileContentResult? getImage(string imageName)
        {
          
            if (!Directory.Exists(directoryPath))
                throw new NotFoundException("image directory not found");

            string? filename = searchFile(imageName);

            if (filename == null)
                return null;

            string imagePath = Path.Combine(directoryPath, filename); // file path          

            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

            string contentType = "image/" + filename.Split('.')[1]; // file type
            return new FileContentResult(imageData, contentType);
                      
        }

        //exception fixed
        public void deleteImage(string imageName)
        {
            //判断文件夹是否存在
            if (!Directory.Exists(directoryPath))
                throw new NotFoundException("image directory not found");

            string? imagePath = searchFile(imageName); 
            if (imagePath!=null&&File.Exists(imagePath))
                File.Delete(imagePath);          
        }


    }
}

