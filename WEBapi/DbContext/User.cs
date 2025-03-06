using System.ComponentModel.DataAnnotations;
namespace WEBapi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class PublicUser
{
    public string Username { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public static class UserExtensions
    {
        public static PublicUser ToPublic(this User user)
        {
            return new PublicUser
            {
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }
    }