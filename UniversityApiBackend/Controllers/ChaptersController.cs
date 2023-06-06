using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApiBackend.DataAccess;
using UniversityApiBackend.Models.DataModels;

namespace UniversityApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly UniversityDBContext _context;

        public ChaptersController(UniversityDBContext context)
        {
            _context = context;
        }

        // GET: api/Chapters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chapters>>> GetChapters()
        {
          if (_context.Chapters == null)
          {
              return NotFound();
          }
            return await _context.Chapters.ToListAsync();
        }

        // GET: api/Chapters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chapters>> GetChapters(int id)
        {
          if (_context.Chapters == null)
          {
              return NotFound();
          }
            var chapters = await _context.Chapters.FindAsync(id);

            if (chapters == null)
            {
                return NotFound();
            }

            return chapters;
        }

        // PUT: api/Chapters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChapters(int id, Chapters chapters)
        {
            if (id != chapters.Id)
            {
                return BadRequest();
            }

            _context.Entry(chapters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChaptersExists(id))
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

        // POST: api/Chapters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Chapters>> PostChapters(Chapters chapters)
        {
          if (_context.Chapters == null)
          {
              return Problem("Entity set 'UniversityDBContext.Chapters'  is null.");
          }
            _context.Chapters.Add(chapters);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChapters", new { id = chapters.Id }, chapters);
        }

        // DELETE: api/Chapters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapters(int id)
        {
            if (_context.Chapters == null)
            {
                return NotFound();
            }
            var chapters = await _context.Chapters.FindAsync(id);
            if (chapters == null)
            {
                return NotFound();
            }

            _context.Chapters.Remove(chapters);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChaptersExists(int id)
        {
            return (_context.Chapters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
