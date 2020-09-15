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
    public class MapsController : ControllerBase
    {
        private readonly gisContext _context;

        public MapsController(gisContext context)
        {
            _context = context;
        }

        // GET: api/Maps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Map>>> GetMap()
        {
            return await _context.Map.ToListAsync();
        }

        // GET: api/Maps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Map>> GetMap(int id)
        {
            var map = await _context.Map.Include("Layer.LayerStyles").Include("Basemap").FirstOrDefaultAsync(m => m.Id == id);

            if (map == null)
            {
                return NotFound();
            }

            map.Layer = map.Layer.OrderBy(l => l.GroupNumber).ThenBy(l => l.LayerNumber).ToList();

            return map;
        }

        // PUT: api/Maps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMap(int id, Map map)
        {
            if (id != map.Id)
            {
                return BadRequest();
            }

            _context.Entry(map).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
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

        // POST: api/Maps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Map>> PostMap(Map map)
        {
            _context.Map.Add(map);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMap", new { id = map.Id }, map);
        }

        // DELETE: api/Maps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Map>> DeleteMap(int id)
        {
            var map = await _context.Map.FindAsync(id);
            if (map == null)
            {
                return NotFound();
            }

            _context.Map.Remove(map);
            await _context.SaveChangesAsync();

            return map;
        }

        private bool MapExists(int id)
        {
            return _context.Map.Any(e => e.Id == id);
        }
    }
}
