using System.Text.Json.Serialization;

namespace opengeo.Models
{
  public class User
  {
    public int Id { get; set; }
    public string DisplayName { get; set; }
    public string photoURL { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
  }
}