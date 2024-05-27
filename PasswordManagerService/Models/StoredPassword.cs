namespace PasswordManagerService.Models
{
    public class StoredPassword
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Site { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
