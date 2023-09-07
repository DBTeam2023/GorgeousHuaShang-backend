using UserIdentification.dto;
namespace UserIdentification.resource.remote
{
    public interface PaymentService
    {
        public void addWallet(TokenDto tokenDto, decimal balance);
    }
}
