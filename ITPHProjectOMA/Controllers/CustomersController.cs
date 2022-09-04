using ITPHProjectOMA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITPHProjectOMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ITPHAcademyOMAContext _context;

        public CustomersController(ITPHAcademyOMAContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }
    }
}
