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
    public class BasemapsController : ControllerBase
    {
        private readonly gisContext _context;

        public BasemapsController(gisContext context)
        {
            _context = context;
        }

        // GET: api/Basemaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basemap>>> GetBasemap()
        {
            return await _context.Basemap.ToListAsync();
        }

        // GET: api/Basemaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basemap>> GetBasemap(int id)
        {
            var basemap = await _context.Basemap.FindAsync(id);

            if (basemap == null)
            {
                return NotFound();
            }

            return basemap;
        }

        // PUT: api/Basemaps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasemap(int id, Basemap basemap)
        {
            if (id != basemap.Id)
            {
                return BadRequest();
            }

            _context.Entry(basemap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasemapExists(id))
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

        // POST: api/Basemaps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Basemap>> PostBasemap(Basemap basemap)
        {
            _context.Basemap.Add(basemap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasemap", new { id = basemap.Id }, basemap);
        }

        // DELETE: api/Basemaps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Basemap>> DeleteBasemap(int id)
        {
            var basemap = await _context.Basemap.FindAsync(id);
            if (basemap == null)
            {
                return NotFound();
            }

            _context.Basemap.Remove(basemap);
            await _context.SaveChangesAsync();

            return basemap;
        }

        private bool BasemapExists(int id)
        {
            return _context.Basemap.Any(e => e.Id == id);
        }
    }
}
