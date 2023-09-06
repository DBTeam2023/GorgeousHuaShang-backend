using UserIdentification.common;
using UserIdentification.dataaccess.DBModels;
using UserIdentification.dataaccess.mapper;
using UserIdentification.exception;

namespace UserIdentification.domain.model.repository.impl
{
    public class UserRepositoryImpl : UserRepository
    {
        ModelContext modelContext;

        public UserRepositoryImpl(ModelContext modelContext)
        {
            this.modelContext = modelContext;
        }

        private User aggregateToUser(UserAggregate user)
        {
            UserType.TypeCheck(user.Type);

            if (user.Type == UserType.Buyer)
            {
                BuyerEntity buyerInfo = user.buyerInfo;
                Buyer buyerPo = new Buyer
                {
                    UserId = user.UserId,
                    Age = buyerInfo.Age,
                    ReceiveAddress = buyerInfo.Address,
                    Gender = buyerInfo.Gender,
                    Height = buyerInfo.Height,
                    Weight = buyerInfo.Weight,
                    IsVip = buyerInfo.IsVip,
                };

                User userPo = new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    NickName = user.NickName,
                    Username = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.Type,
                    Buyer = buyerPo
                };

                return userPo;
            }
            else if (user.Type == UserType.Seller)
            {
                SellerEntity sellerInfo = user.sellerInfo;
                Seller sellerPo = new Seller
                {
                    UserId = user.UserId,
                    SendAddress = sellerInfo.Address
                };

                User userPo = new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    NickName = user.NickName,
                    Username = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.Type,
                    Seller = sellerPo
                };

                return userPo;
            }
            else if(user.Type == UserType.Administrator)
            {
                User userPo = new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    NickName = user.NickName,
                    Username = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.Type
                };

                return userPo;
            }

            throw new InvalidTypeException("invalid user aggregate type transfer");
        }

        public void add(UserAggregate user)
        {
            //security check
            UserType.TypeCheck(user.Type);
            var existUser = modelContext.Users.Where(x => x.Username == user.Username).FirstOrDefault();
            if (existUser != null)
            {
                throw new DuplicateException("username already exists");
            }

            //save specialization info
            if (user.Type == UserType.Buyer)
            {
                BuyerEntity buyerInfo = user.buyerInfo;
                Buyer buyerPo = new Buyer
                {
                    UserId = user.UserId,
                    Age = buyerInfo.Age,
                    ReceiveAddress = buyerInfo.Address,
                    Gender = buyerInfo.Gender,
                    Height = buyerInfo.Height,
                    Weight = buyerInfo.Weight,
                    IsVip = buyerInfo.IsVip,
                };

                User userPo = new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    NickName = user.NickName,
                    Username = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.Type,
                    Buyer = buyerPo
                };

                //modelContext.Buyers.Add(buyerPo);
                modelContext.Users.Add(userPo);
            }
            else if (user.Type == UserType.Seller)
            {
                SellerEntity sellerInfo = user.sellerInfo;
                Seller sellerPo = new Seller
                {
                    UserId= user.UserId,
                    SendAddress = sellerInfo.Address
                };

                User userPo = new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    NickName = user.NickName,
                    Username = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Type = user.Type,
                    Seller = sellerPo
                };

                //modelContext.Sellers.Add(sellerPo);
                modelContext.Users.Add(userPo);
            }
            else { } //administrator

            modelContext.SaveChanges();
        }

        public void update(UserAggregate userAggregate)
        {
            //security check
            User updateUser = modelContext.Users.Where(x => x.UserId == userAggregate.UserId).FirstOrDefault();
            if (updateUser == null)
            {
                throw new NotFoundException("user not found");
            }

            //transfer
            User updateInfo = aggregateToUser(userAggregate);

            //set values
            modelContext.Entry(updateUser).CurrentValues.SetValues(updateInfo);
            if (userAggregate.Type == UserType.Buyer) updateUser.Buyer = updateInfo.Buyer;
            else if (userAggregate.Type == UserType.Seller) updateUser.Seller = updateInfo.Seller;

            //set entity status
            modelContext.Entry(updateUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            if (userAggregate.Type == UserType.Buyer) modelContext.Entry(updateUser.Buyer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            else if (userAggregate.Type == UserType.Seller) modelContext.Entry(updateUser.Seller).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            modelContext.SaveChanges();
        }

        public void delete(string userId)
        {

        }

        public UserAggregate getById(string userId)
        {
            //get user info
            User userInfo = modelContext.Users.FirstOrDefault(e => e.UserId == userId);

            if (userInfo == null)
            {
                LoginException exception = new LoginException("user not found");
                exception.UserNotFound = true;
                throw exception;
            }

            //get detail info for account
            UserAggregate userAggregate = new UserAggregate(userInfo.UserId,userInfo.NickName,userInfo.PhoneNumber,userInfo.Username, userInfo.Password, userInfo.Type);
            if(userInfo.Type == UserType.Buyer)
            {
                Buyer buyerPo = modelContext.Buyers.FirstOrDefault(e => e.UserId == userId);
                userAggregate.buyerInfo = new BuyerEntity
                {
                    UserId = buyerPo.UserId,
                    Address = buyerPo.ReceiveAddress,
                    Age = buyerPo.Age,
                    Gender = buyerPo.Gender,
                    Height = buyerPo.Height,
                    Weight = buyerPo.Weight,
                    IsVip = buyerPo.IsVip
                };
            }
            else if(userInfo.Type == UserType.Seller)
            {
                Seller sellerPo = modelContext.Sellers.FirstOrDefault(e => e.UserId == userId);
                userAggregate.sellerInfo = new SellerEntity
                {
                    UserId = sellerPo.UserId,
                    Address = sellerPo.SendAddress
                };
            }
            else { }  //administrator

            return userAggregate;
        }

        public UserAggregate getByUsername(string username)
        {
            User userInfo = modelContext.Users.FirstOrDefault(e => e.Username == username);
            if(userInfo == null)
            {
                throw new NotFoundException("user not found");
            }

            return getById(userInfo.UserId);
        }
    }
}
