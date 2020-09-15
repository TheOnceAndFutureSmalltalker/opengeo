using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opengeo.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opengeo.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StylesController : ControllerBase
  {
    private readonly gisContext _context;

    public StylesController(gisContext context)
    {
      _context = context;
    }


    // get a style by id
    [HttpGet("{id}")]
    public async Task<ActionResult<LayerStyles>> GetStyles(int id)
    {
      var styles = await _context.LayerStyles.FindAsync(id);

      if (styles == null)
      {
        return NotFound();
      }

      return styles;
    }

    // get collection of styles for layer
    [HttpGet("layer/{layer_id}")]
    public async Task<ActionResult<IEnumerable<LayerStyles>>> GetLayerStyles(int layer_id)
    {
      return await _context.LayerStyles.Where(ls=>ls.LayerId == layer_id).ToListAsync();
    }

    

    // POST api/<StylesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<StylesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<StylesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
