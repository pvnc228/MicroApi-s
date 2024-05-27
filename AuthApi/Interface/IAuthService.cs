using AuthApi.Models;

namespace AuthApi.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterModel model);
        Task<(bool Success, string Token)> LoginAsync(LoginModel model);
        Task<bool> UserExistsAsync(string username);
    }
}
