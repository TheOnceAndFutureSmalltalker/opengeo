using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opengeo.Models;

namespace opengeo.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LayersController : ControllerBase
  {
    private readonly gisContext _context;

    public LayersController(gisContext context)
    {
      _context = context;
    }

    // GET: api/Layers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Layer>>> GetLayer()
    {
      return await _context.Layer.ToListAsync();
    }

    // GET: api/Layers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Layer>> GetLayer(int id)
    {
        var layer = await _context.Layer.FindAsync(id);

        if (layer == null)
        {
            return NotFound();
        }

        return layer;
    }

    // PUT: api/Layers/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLayer(int id, Layer layer)
    {
        if (id != layer.Id)
        {
            return BadRequest();
        }

        _context.Entry(layer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LayerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Layers
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Layer>> PostLayer(Layer layer)
    {
        _context.Layer.Add(layer);
        await _context.SaveChangesAsync();

      

        return CreatedAtAction("GetLayer", new { id = layer.Id }, layer);
    }

    // DELETE: api/Layers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Layer>> DeleteLayer(int id)
    {
        var layer = await _context.Layer.FindAsync(id);
        if (layer == null)
        {
            return NotFound();
        }

        _context.Layer.Remove(layer);
        await _context.SaveChangesAsync();

        return layer;
    }

    private bool LayerExists(int id)
    {
        return _context.Layer.Any(e => e.Id == id);
    }
  }
}
