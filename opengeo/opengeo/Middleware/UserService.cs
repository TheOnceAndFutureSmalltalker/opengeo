using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using opengeo.Models;

namespace opengeo.Middleware
{
  public interface IUserService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    AuthenticateResponse AuthenticateWithToken(string token);
    AuthenticateResponse RegisterUser(RegisterRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
  }

  public class UserService : IUserService
  {
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new List<User>
        {
            new User { Id = 1, DisplayName = "Test", photoURL = "User", Username = "test", Password = "test" }
        };

    private readonly AppSettings _appSettings;
    private readonly gisContext _context;

    public UserService(IOptions<AppSettings> appSettings, gisContext context)
    {
      _appSettings = appSettings.Value;
      _context = context;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
      var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

      user = _context.User.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

      // return null if user not found
      if (user == null) return null;

      // authentication successful so generate jwt token
      var token = generateJwtToken(user);

      return new AuthenticateResponse(user, token);
    }


    public IEnumerable<User> GetAll()
    {
      return _context.User.ToArray();
    }

    public User GetById(int id)
    {
      return _context.User.FirstOrDefault(x => x.Id == id);
    }

    public AuthenticateResponse AuthenticateWithToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var jwtToken = (JwtSecurityToken)validatedToken;
      var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

      User user = GetById(userId);
      return new AuthenticateResponse(user, token);
    }

    public AuthenticateResponse RegisterUser(RegisterRequest model)
    {
      // verify we have username, password, email, displayName
      // check pre-existing email/username
      User user = new User()
      {
        Username = model.username,
        Password = model.password,
        Email = model.email,
        DisplayName = model.displayName
      };
      _context.User.Add(user);
      _context.SaveChanges();

      string token = generateJwtToken(user);
      return new AuthenticateResponse(user, token);
    }


    // helper methods

    private string generateJwtToken(User user)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}