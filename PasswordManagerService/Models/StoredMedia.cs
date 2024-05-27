namespace PasswordManagerService.Models
{
    public class StoredMedia
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string MediaType { get; set; } // "photo" or "video"
        public byte[] Content { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
