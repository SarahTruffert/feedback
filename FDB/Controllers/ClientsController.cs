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
    [RestrictDomain("https://feedback-simplon.azurewebsites.net/")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly FDBContext _context;

        public ClientsController(FDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientsDto>>> GetClientsDto()
        {
            return await _context.ClientsDto.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientsDto>> GetClientsDto(Guid id)
        {
            var clientsDto = await _context.ClientsDto.FindAsync(id);

            if (clientsDto == null)
            {
                return NotFound();
            }

            return clientsDto;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientsDto(Guid id, ClientsDto clientsDto)
        {
            if (id != clientsDto.User_Id)
            {
                return BadRequest();
            }

            _context.Entry(clientsDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientsDtoExists(id))
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

        [HttpPost]
        public async Task<ActionResult<ClientsDto>> PostClientsDto(ClientsDto clientsDto)
        {
            _context.ClientsDto.Add(clientsDto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientsDtoExists(clientsDto.User_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClientsDto", new { id = clientsDto.User_Id }, clientsDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientsDto(Guid id)
        {
            var clientsDto = await _context.ClientsDto.FindAsync(id);
            if (clientsDto == null)
            {
                return NotFound();
            }

            _context.ClientsDto.Remove(clientsDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientsDtoExists(Guid id)
        {
            return _context.ClientsDto.Any(e => e.User_Id == id);
        }
    }
}
