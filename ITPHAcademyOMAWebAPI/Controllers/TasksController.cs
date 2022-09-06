using ITPHAcademyOMAWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITPHAcademyOMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITPHAcademyOMAContext _context;

        public TasksController(ITPHAcademyOMAContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.Select(x => new
            {
                x.Id,
                x.IsDone,
                comments = x.Comments.ToList(),
                x.StartDate,
                x.EndDate,
                user = x.User.Name + " " + x.User.Surname,
                x.UserId,
                x.Description,
                x.Project.Name,
                x.ProjectId
            }

                ).ToListAsync();
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks
        [HttpGet("{projectid}/tasks")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasksbyProject(int projectid)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.Where(x => x.ProjectId == projectid).Select(x => new
            {
                x.Id,
                x.IsDone,
                comments = x.Comments.ToList(),
                x.StartDate,
                x.EndDate,
                user = x.User.Name + " " + x.User.Surname,
                x.UserId,
                x.Description,
                x.Project.Name,
                x.ProjectId
            }

                ).ToListAsync();
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.IsDone,
                comments = x.Comments.Select(c => new
                {
                    c.Id,
                    c.Comment1,
                    c.User.Name,
                    c.User.Surname,
                    c.UserId,
                    c.User.Username,
                    c.TaskId
                }),
                x.StartDate,
                x.EndDate,
                user = x.User.Name + " " + x.User.Surname,
                x.UserId,
                x.Description,
                x.Project.Name,
                x.ProjectId
            }

                ).FirstOrDefaultAsync();


            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ITPHAcademyOMAContext.Tasks'  is null.");
            }
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
