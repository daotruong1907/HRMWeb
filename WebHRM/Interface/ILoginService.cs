using WebHRM.DTO;

namespace WebHRM.Interface
{
    public interface ILoginService
    {
        ResponseLogin Login(LoginDto loginDto);
    }
}
