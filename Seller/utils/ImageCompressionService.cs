using ImageMagick;
using Storesys.exception;

namespace Storesys.utils
{
    public class ImageCompressionService
    {
        public IFormFile? Image { get; set; }

        public string Path { get; set; }

        public ImageCompressionService(IFormFile? image, string path)
        {
            Image = image;
            Path = path;
        }

        private MagickImage ConvertToMagickImage(IFormFile file)
        {
            try
            {
                using (Stream stream = file.OpenReadStream())
                {
                    var image = new MagickImage(stream);
                    return image;
                }
            }
            catch
            {
                throw new ConvertException("IFormFile to image failure");
            }

        }


        public void ProcessImage(int Kb)
        {

            if (Image == null)
                return;

            // 加载源图像
            using (MagickImage image = ConvertToMagickImage(Image))
            {
                CompressImage(Kb * 1024);
            }
        }



        private void CompressImage(int targetSizeInBytes)
        {
            int quality = 100;
            int width = 0;
            int height = 0;

            using (var image = new MagickImage(Image.OpenReadStream()))
            {
                while (image.Height * image.Width * image.Depth / 8 > targetSizeInBytes)
                {
                    // 减小压缩质量
                    if (quality > 10)
                    {
                        quality -= 10;
                        image.Quality = quality;
                    }


                    // 根据需要调整图像尺寸
                    if (width == 0 && height == 0)
                    {
                        width = image.Width;
                        height = image.Height;
                    }

                    width = (int)(width * 0.8);
                    height = (int)(height * 0.8);
                    image.Resize(width, height);
                }

                image.Write(Path);
            }
        }



    }
}
