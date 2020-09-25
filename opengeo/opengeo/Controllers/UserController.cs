using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using opengeo.Models;
using opengeo.Middleware;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opengeo.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
      var response = _userService.Authenticate(model);

      if (response == null)
        return BadRequest(new { message="Bad credentials." });

      return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Refresh()
    {
      User user = (User)HttpContext.Items["User"];
      var response = _userService.Refresh(user);
      if (response == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public IActionResult All()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }
  }
}
