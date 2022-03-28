using WebHRM.DTO;

namespace WebHRM.Interface
{
    public interface ILoginService
    {
        ResponseLoginClone Login(LoginDto loginDto);
    }
}
