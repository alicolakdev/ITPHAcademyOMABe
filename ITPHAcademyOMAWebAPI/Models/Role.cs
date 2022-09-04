using System;
using System.Collections.Generic;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            Customers = new HashSet<Customer>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
