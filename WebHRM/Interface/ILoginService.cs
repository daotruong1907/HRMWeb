using WebHRM.DTO;

namespace WebHRM.Interface
{
    public interface ILoginService
    {
        bool Login(LoginDto loginDto);
    }
}
