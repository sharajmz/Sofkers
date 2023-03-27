using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Context;
using Domain.Entities;

namespace Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationTypesController : ControllerBase
    {
        private readonly PeopleDevSofkaContext _context;

        public IdentificationTypesController(PeopleDevSofkaContext context)
        {
            _context = context;
        }

        // GET: api/IdentificationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentificationType>>> GetIdentificationTypes()
        {
          if (_context.IdentificationTypes == null)
          {
              return NotFound();
          }
            return await _context.IdentificationTypes.ToListAsync();
        }

        // GET: api/IdentificationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentificationType>> GetIdentificationType(int id)
        {
          if (_context.IdentificationTypes == null)
          {
              return NotFound();
          }
            var identificationType = await _context.IdentificationTypes.FindAsync(id);

            if (identificationType == null)
            {
                return NotFound();
            }

            return identificationType;
        }

        // PUT: api/IdentificationTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentificationType(int id, IdentificationType identificationType)
        {
            if (id != identificationType.IdentificationId)
            {
                return BadRequest();
            }

            _context.Entry(identificationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentificationTypeExists(id))
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

        // POST: api/IdentificationTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IdentificationType>> PostIdentificationType(IdentificationType identificationType)
        {
          if (_context.IdentificationTypes == null)
          {
              return Problem("Entity set 'PeopleDevSofkaContext.IdentificationTypes'  is null.");
          }
            _context.IdentificationTypes.Add(identificationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIdentificationType", new { id = identificationType.IdentificationId }, identificationType);
        }

        // DELETE: api/IdentificationTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentificationType(int id)
        {
            if (_context.IdentificationTypes == null)
            {
                return NotFound();
            }
            var identificationType = await _context.IdentificationTypes.FindAsync(id);
            if (identificationType == null)
            {
                return NotFound();
            }

            _context.IdentificationTypes.Remove(identificationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IdentificationTypeExists(int id)
        {
            return (_context.IdentificationTypes?.Any(e => e.IdentificationId == id)).GetValueOrDefault();
        }
    }
}
