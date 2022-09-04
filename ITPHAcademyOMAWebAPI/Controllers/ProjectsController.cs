using ITPHAcademyOMAWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITPHAcademyOMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ITPHAcademyOMAContext _context;

        public ProjectsController(ITPHAcademyOMAContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.Select(x => new
            {
                x.Id,
                x.Name,
                x.CustomerId,
                customerName = x.Customer.Name,
                customerSurname = x.Customer.Surname,
                customerUsername = x.Customer.Username,
                taskCount = x.Tasks.Count
            }).ToListAsync();

            return Ok(projects);

        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }

            var projectDetails = await _context.Projects.Where(p => p.Id == id)
                .Include(p => p.Customer)
                .Include(p => p.Tasks).ThenInclude(t => t.Comments)
                .Select(p => new Project
                {
                    Id = p.Id,
                    Name = p.Name,
                    Customer = new Customer { Id = p.Customer.Id, Name = p.Customer.Name, Surname = p.Customer.Surname, Username = p.Customer.Username, RoleId = p.Customer.RoleId },
                    Tasks = p.Tasks

                })
                .ToListAsync();

            //var project = await _context.Projects.FindAsync(id);

            if (projectDetails == null)
            {
                return NotFound();
            }

            return Ok(projectDetails);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ITPHAcademyOMAContext.Projects'  is null.");
            }
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
