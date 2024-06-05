namespace PasswordManagerService.Models
{
    public class PasswordModel{
       public required string Username { get; set; } 
       public string Site { get; set; } 
       public required string Password { get; set; }
    }
}
