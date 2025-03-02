using System.ComponentModel.DataAnnotations;
namespace WEBapi.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; } = "";
    public DateTime Created { get; set; }
}
