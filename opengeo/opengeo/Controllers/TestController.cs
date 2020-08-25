using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opengeo.Controllers
{
  
  public class TestController : Controller
  {
    // GET: api/<TestController>
    [HttpGet]
    [Route("api/Test")]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

   

    // POST api/<TestController>
    [HttpPost]
    [Route("api/Test")]
    public async Task<ActionResult> Post()
    {
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        string body =  await reader.ReadToEndAsync();
        
      }
      int i = 10;
      return Content(i.ToString());

    }

  }
}
