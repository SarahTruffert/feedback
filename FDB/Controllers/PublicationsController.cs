#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FDB.DataContext;
using FDB.Mod;

namespace FDB.Controllers
{
    [Route("api/[controller]")]
    [RestrictDomain("localhost")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly FDBContext _context;

        public PublicationsController(FDBContext context)
        {
            _context = context;
        }

        // GET: api/Publications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicationsDto>>> GetPublicationDto()
        {
            return await _context.PublicationDto.ToListAsync();
        }

        // GET: api/Publications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicationsDto>> GetPublicationsDto(Guid id)
        {
            var publicationsDto = await _context.PublicationDto.FindAsync(id);

            if (publicationsDto == null)
            {
                return NotFound();
            }

            return publicationsDto;
        }

        // PUT: api/Publications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublicationsDto(Guid id, PublicationsDto publicationsDto)
        {
            if (id != publicationsDto.Publication_Id)
            {
                return BadRequest();
            }

            _context.Entry(publicationsDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationsDtoExists(id))
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

        // POST: api/Publications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PublicationsDto>> PostPublicationsDto(PublicationsDto publicationsDto)
        {
            _context.PublicationDto.Add(publicationsDto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PublicationsDtoExists(publicationsDto.Publication_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPublicationsDto", new { id = publicationsDto.Publication_Id }, publicationsDto);
        }

        // DELETE: api/Publications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicationsDto(Guid id)
        {
            var publicationsDto = await _context.PublicationDto.FindAsync(id);
            if (publicationsDto == null)
            {
                return NotFound();
            }

            _context.PublicationDto.Remove(publicationsDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublicationsDtoExists(Guid id)
        {
            return _context.PublicationDto.Any(e => e.Publication_Id == id);
        }
    }
}
