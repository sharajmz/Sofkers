using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Context;
using Domain.Entities;
using Application.Interfaces;
using Domain.DTOs;

namespace Infrastructure.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SofkersController : ControllerBase
    {
        private readonly PeopleDevSofkaContext _context;
        private IServiceQueueBus _queueService;

        public SofkersController(PeopleDevSofkaContext context, IServiceQueueBus queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        // GET: api/Sofkers
        [HttpGet]
        public async Task<ActionResult<List<Sofker>>> GetSofkers()
        {
          if (_context.Sofkers == null)
          {
              return NotFound();
          }
            return await _context.Sofkers.ToListAsync();
        }

        // GET: api/Sofkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sofker>> GetSofker(int id)
        {
          if (_context.Sofkers == null)
          {
              return NotFound();
          }
            var sofker = await _context.Sofkers.FindAsync(id);

            if (sofker == null)
            {
                return NotFound();
            }

            return sofker;
        }

        // PUT: api/Sofkers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSofker(int id, Sofker sofker)
        {
            if (id != sofker.SofkerTypeId)
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
          if (_context.Sofkers == null)
          {
              return Problem("Entity set 'PeopleDevSofkaContext.Sofkers'  is null.");
          }
            SofkerStadistic sofkerStadistic = new SofkerStadistic()
            {
                Identification = _context.IdentificationTypes.FirstOrDefault(t => t.IdentificationId == sofker.SofkerTypeId).Identification + sofker.SofkerId.ToString(),
                Name = sofker.SofkerName,
                ChangesDatetime = DateTime.Now,
                IsSofkerActive = sofker.SofkerActive,
                SofkerClient = _context.Clients.FirstOrDefault(x => x.ClientId == sofker.SofkerClient).ClientName
            };
            _context.Sofkers.Add(sofker);
            try
            {
                var _addResult = await _context.SaveChangesAsync();
                if (_addResult > 0)
                {
                    await _queueService.QueueMessagesAsync(sofkerStadistic);
                }
            }
            catch (DbUpdateException)
            {
                if (SofkerExists(sofker.SofkerTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSofker", new { id = sofker.SofkerTypeId }, sofker);
        }

        // DELETE: api/Sofkers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSofker(int id)
        {
            if (_context.Sofkers == null)
            {
                return NotFound();
            }
            var sofker = await _context.Sofkers.FindAsync(id);
            if (sofker == null)
            {
                return NotFound();
            }

            _context.Sofkers.Remove(sofker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SofkerExists(int id)
        {
            return (_context.Sofkers?.Any(e => e.SofkerTypeId == id)).GetValueOrDefault();
        }
    }
}
