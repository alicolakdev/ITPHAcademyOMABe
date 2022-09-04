using System;
using System.Collections.Generic;

namespace ITPHProjectOMA.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsCanceled { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
