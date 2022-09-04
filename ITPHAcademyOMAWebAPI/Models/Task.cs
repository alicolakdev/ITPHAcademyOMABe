using System;
using System.Collections.Generic;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class Task
    {
        public Task()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public bool IsDone { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
