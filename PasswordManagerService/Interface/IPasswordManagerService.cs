using PasswordManagerService.Models;

namespace PasswordManagerService.Interface
{
    public interface IPasswordManagerService
    {
        Task<(bool Success, string Message)> StorePasswordAsync(PasswordModel model);
        Task<IEnumerable<PasswordModel>> RetrievePasswordsAsync(string username);
        Task<(bool Success, string Message)> StoreMediaAsync(MediaModel model);
        Task<IEnumerable<MediaModel>> RetrieveMediaAsync(string username);
    }
}
