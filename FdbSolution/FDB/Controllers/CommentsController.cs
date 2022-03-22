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
    [RestrictDomain("https://feedback-webapp.azurewebsites.net")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly FDBContext _context;

        public CommentsController(FDBContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetCommentsDto()
        {
            return await _context.CommentsDto.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentsDto>> GetCommentsDto(Guid id)
        {
            var commentsDto = await _context.CommentsDto.FindAsync(id);

            if (commentsDto == null)
            {
                return NotFound();
            }

            return commentsDto;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommentsDto(Guid id, CommentsDto commentsDto)
        {
            if (id != commentsDto.Comment_Id)
            {
                return BadRequest();
            }

            _context.Entry(commentsDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentsDtoExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentsDto>> PostCommentsDto(CommentsDto commentsDto)
        {
            _context.CommentsDto.Add(commentsDto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CommentsDtoExists(commentsDto.Comment_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCommentsDto", new { id = commentsDto.Comment_Id }, commentsDto);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentsDto(Guid id)
        {
            var commentsDto = await _context.CommentsDto.FindAsync(id);
            if (commentsDto == null)
            {
                return NotFound();
            }

            _context.CommentsDto.Remove(commentsDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentsDtoExists(Guid id)
        {
            return _context.CommentsDto.Any(e => e.Comment_Id == id);
        }
    }
}
