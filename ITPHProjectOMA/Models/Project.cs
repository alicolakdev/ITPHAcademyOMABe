using System;
using System.Collections.Generic;

namespace ITPHProjectOMA.Models
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
