using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capybara_api.Infra;
using capybara_api.Models;

namespace capybara_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PreferencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Preferences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preference>>> GetPreference()
        {
          if (_context.Preference == null)
          {
              return NotFound();
          }
            return await _context.Preference.ToListAsync();
        }

        // GET: api/Preferences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preference>> GetPreference(Guid id)
        {
          if (_context.Preference == null)
          {
              return NotFound();
          }
            var preference = await _context.Preference.FindAsync(id);

            if (preference == null)
            {
                return NotFound();
            }

            return preference;
        }

        // PUT: api/Preferences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreference(Guid id, Preference preference)
        {
            if (id != preference.Id)
            {
                return BadRequest();
            }

            _context.Entry(preference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenceExists(id))
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

        // POST: api/Preferences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preference>> PostPreference(Preference preference)
        {
          if (_context.Preference == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Preference'  is null.");
          }
            _context.Preference.Add(preference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreference", new { id = preference.Id }, preference);
        }

        // DELETE: api/Preferences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreference(Guid id)
        {
            if (_context.Preference == null)
            {
                return NotFound();
            }
            var preference = await _context.Preference.FindAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            _context.Preference.Remove(preference);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreferenceExists(Guid id)
        {
            return (_context.Preference?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
