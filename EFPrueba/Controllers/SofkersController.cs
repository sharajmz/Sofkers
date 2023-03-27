using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using EFPrueba.Data;

namespace EFPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SofkersController : ControllerBase
    {
        private readonly EFPruebaContext _context;

        public SofkersController(EFPruebaContext context)
        {
            _context = context;
        }

        // GET: api/Sofkers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sofker>>> GetSofker()
        {
          if (_context.Sofker == null)
          {
              return NotFound();
          }
            return await _context.Sofker.ToListAsync();
        }

        // GET: api/Sofkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sofker>> GetSofker(string id)
        {
          if (_context.Sofker == null)
          {
              return NotFound();
          }
            var sofker = await _context.Sofker.FindAsync(id);

            if (sofker == null)
            {
                return NotFound();
            }

            return sofker;
        }

        // PUT: api/Sofkers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSofker(string id, Sofker sofker)
        {
            if (id != sofker.SofkerId)
            {
                return BadRequest();
            }

            _context.Entry(sofker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SofkerExists(id))
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

        // POST: api/Sofkers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sofker>> PostSofker(Sofker sofker)
        {
          if (_context.Sofker == null)
          {
              return Problem("Entity set 'EFPruebaContext.Sofker'  is null.");
          }
            _context.Sofker.Add(sofker);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SofkerExists(sofker.SofkerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSofker", new { id = sofker.SofkerId }, sofker);
        }

        // DELETE: api/Sofkers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSofker(string id)
        {
            if (_context.Sofker == null)
            {
                return NotFound();
            }
            var sofker = await _context.Sofker.FindAsync(id);
            if (sofker == null)
            {
                return NotFound();
            }

            _context.Sofker.Remove(sofker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SofkerExists(string id)
        {
            return (_context.Sofker?.Any(e => e.SofkerId == id)).GetValueOrDefault();
        }
    }
}
