

namespace opengeo.Models
{
  // these entities define the data model that Fuse expects
  public class AuthenticatedUserData
  {
    public AuthenticatedUserData(User user)
    {
      displayName = user.DisplayName;
      photoURL = user.photoURL;
      username = user.Username;
      email = user.Email;
    }
    public string displayName { get; set; }
    public string photoURL { get; set; }
    public string username { get; set; }
    public string email { get; set; }
  }
  public class AuthenticatedUser
  {
    public AuthenticatedUser(User user)
    {
      id = user.Id;
      from = "opengeo";
      role = "user";
      data = new AuthenticatedUserData(user);
    }
    public int id { get; set; }
    public string from { get; set; }
    public string role { get; set; }
    public AuthenticatedUserData data { get; set; }
  }
  public class AuthenticateResponse
  {
    public AuthenticatedUser user { get; set; }
    public string access_token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
      this.user = new AuthenticatedUser(user);
      this.access_token = token;  
    }
  }
}
