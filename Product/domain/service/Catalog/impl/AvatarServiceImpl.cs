using Microsoft.AspNetCore.Mvc;
using Product.exception;
using Product.utils;

namespace Product.domain.service.impl
{
    public class AvatarServiceImpl : AvatarService
    {

        public readonly static string CommodityImageDirectory = "C:\\Users\\Administrator\\Desktop\\image\\Commodity";

        public readonly static string PickImageDirectory = "C:\\Users\\Administrator\\Desktop\\image\\Pick";

        //for local test path
        //public readonly static string CommodityImageDirectory = "D:\\Test\\Commodity";

        //public readonly static string PickImageDirectory = "D:\\Test\\Pick";
        public AvatarServiceImpl() { }

        public void setCommodityAvatar(IFormFile? image, string avatarName)
        {
            ImageService imageService = new ImageService(CommodityImageDirectory);
            imageService.setImage(image, avatarName);
            
        }

        public void setPickAvatar(IFormFile? image, string avatarName)
        {
    
            ImageService imageService = new ImageService(PickImageDirectory);
            imageService.setImage(image, avatarName);
            
        }


        public FileContentResult getCommodityAvatar(string avatarName)
        {
           
            ImageService imageService = new ImageService(CommodityImageDirectory);
            var result = imageService.getImage(avatarName);
            if (result != null)
                return result;
          
                
            else           
                return imageService.getImage("default.png");

                         
        }

        public FileContentResult getPickAvatar(string avatarName,string upperAvatarName)
        {
            
            ImageService imageService = new ImageService(PickImageDirectory);
            var result = imageService.getImage(avatarName);
            if (result != null)           
                return result;             
            else
                return getCommodityAvatar(upperAvatarName);
        }





        public void deleteCommodityAvatar(string avatarName)
        {
           
            ImageService imageService = new ImageService(CommodityImageDirectory);
            imageService.deleteImage(avatarName);
           
        }

        public void deletePickAvatar(string avatarName)
        {
            
            ImageService imageService = new ImageService(PickImageDirectory);
            imageService.deleteImage(avatarName);
            
        }


    }

}
