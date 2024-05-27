using Microsoft.EntityFrameworkCore;
using PasswordManagerService.Data;
using PasswordManagerService.Models;

namespace PasswordManagerService.Class
{
    public class PasswordManagerServiceClass
    {
        private readonly PasswordManagerContext _context;

        public PasswordManagerServiceClass(PasswordManagerContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> StorePasswordAsync(PasswordModel model)
        {
            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var storedPassword = new StoredPassword
                {
                    Username = model.Username,
                    Site = model.Site,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow
                };

                _context.StoredPasswords.Add(storedPassword);
                await _context.SaveChangesAsync();

                return (true, "Password stored successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to store password: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PasswordModel>> RetrievePasswordsAsync(string username)
        {
            var passwords = await _context.StoredPasswords
                .Where(p => p.Username == username)
                .Select(p => new PasswordModel(p.Username, p.Site, p.PasswordHash))
                .ToListAsync();

            return passwords;
        }

        public async Task<(bool Success, string Message)> StoreMediaAsync(MediaModel model)
        {
            try
            {
                var storedMedia = new StoredMedia
                {
                    Username = model.Username,
                    MediaType = model.MediaType,
                    Content = model.Content,
                    UploadedAt = DateTime.UtcNow
                };

                _context.StoredMedia.Add(storedMedia);
                await _context.SaveChangesAsync();

                return (true, "Media stored successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to store media: {ex.Message}");
            }
        }

        public async Task<IEnumerable<MediaModel>> RetrieveMediaAsync(string username)
        {
            var mediaFiles = await _context.StoredMedia
                .Where(m => m.Username == username)
                .Select(m => new MediaModel(m.Username, m.MediaType, m.Content))
                .ToListAsync();

            return mediaFiles;
        }
    }
}
