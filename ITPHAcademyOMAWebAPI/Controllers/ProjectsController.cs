using ITPHAcademyOMAWebAPI.Models;
using ITPHAcademyOMAWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ITPHAcademyOMAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ITPHAcademyOMAContext _context;
        private ITokenService _tokenService;
        public ProjectsController(ITPHAcademyOMAContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Projects
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            string token = _tokenService.GetToken(HttpContext.Request.Headers[HeaderNames.Authorization]);
            var user = _tokenService.GetUser(token);

            if ((Roles)user.RoleId == Roles.CUSTOMER)
            {

                var projects1 = await _context.Projects
                    .Where(x => x.Customer.Id == user.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.CustomerId,
                        customerName = x.Customer.Name,
                        customerSurname = x.Customer.Surname,
                        customerUsername = x.Customer.Username,
                        taskCount = x.Tasks.Count
                    }).ToListAsync();

                return Ok(projects1);
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }




            var projectDetails = await _context.Projects
                .Include(p => p.Customer)
                .Include(p => p.Tasks).ThenInclude(t => t.Comments)
                .Select(p => new Project
                {
                    Id = p.Id,
                    Name = p.Name,
                    Customer = new User { Id = p.Customer.Id, Name = p.Customer.Name, Surname = p.Customer.Surname, Username = p.Customer.Username, RoleId = p.Customer.RoleId },
                    Tasks = p.Tasks

                }).FirstOrDefaultAsync(p => p.Id == id);

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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
