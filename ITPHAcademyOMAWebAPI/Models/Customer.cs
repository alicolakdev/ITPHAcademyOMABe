using System;
using System.Collections.Generic;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Project> Projects { get; set; }
    }
}
