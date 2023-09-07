using UserIdentification.dto;
namespace UserIdentification.resource.remote
{
    public interface PaymentService
    {
        public Task addWallet(TokenDto tokenDto, decimal balance);
    }
}
