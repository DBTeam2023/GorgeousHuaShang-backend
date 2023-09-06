using Microsoft.VisualBasic;
using Product.domain.model;
using Product.domain.model.repository;
using Product.domain.service;
using Product.dto;

namespace Product.application.impl
{
    public class CartApplicationServiceImpl :CartApplicationService
    {
        private CartRepository _cartRepository ;
        private ProductService _productService;
        private AvatarService _avatarService;

        //这里依赖注入可以改一下
        public CartApplicationServiceImpl(CartRepository cartRepository,ProductService productService,AvatarService avatarService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
            _avatarService = avatarService;
        }

        public async Task addCartItem(string userId, PickCountDto pick)
        {
            await _cartRepository.addItem(userId, pick);

        }

        public async Task deleteCartItem(string userId, PickIdDto pickId)
        {
            await _cartRepository.deletePick(userId, pickId.PickId);

        }

        public async Task deleteCartItem(string userId, List<PickIdDto> picks)
        {
            foreach (var pick in picks)
            {
                await _cartRepository.deletePick(userId, pick.PickId);
            }
        }

        //public async Task updateCartItem(string userId, ChangePickDto changePickDto )
        //{
        //    await _cartRepository.changePick(userId, oldPickId.PickId, newPick);

        //}




    }
}
