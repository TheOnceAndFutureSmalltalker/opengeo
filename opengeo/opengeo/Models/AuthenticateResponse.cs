

namespace opengeo.Models
{
  public class AuthenticatedUser
  {
    public AuthenticatedUser(User user)
    {
      id = user.Id;
      firstname = user.FirstName;
      lastname = user.LastName;
      username = user.Username;
    }
    public int id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string username { get; set; }
  }
  public class AuthenticateResponse
  {
    public AuthenticatedUser user { get; set; }
    public string access_token { get; set; }


    public AuthenticateResponse(User user, string access_token)
    {
      this.user = new AuthenticatedUser(user);
      this.access_token = access_token;
    }
  }
}
