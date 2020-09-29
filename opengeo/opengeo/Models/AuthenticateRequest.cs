using System.ComponentModel.DataAnnotations;

namespace opengeo.Models
{
  public class AuthenticateRequest
  {
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
  }

  public class AuthenticateTokenRequest
  {
    [Required]
    public string access_token { get; set; }
  }
 
  public class RegisterRequest
  {
    public string displayName { get; set; }
    public string password { get; set; }
    public string username { get; set; }
    public string email { get; set; }
  }
}