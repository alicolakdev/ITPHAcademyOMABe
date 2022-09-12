using System;
using System.Collections.Generic;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;


        public virtual ICollection<User> Users { get; set; }
    }

    public enum Roles
    {
        LEADER = 1,
        DEVELOPER = 2,
        CUSTOMER = 3
    }
}
