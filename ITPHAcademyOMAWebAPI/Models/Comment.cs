using System;
using System.Collections.Generic;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Comment1 { get; set; } = null!;
        public int TaskId { get; set; }
        public int UserId { get; set; }

        public virtual Task Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
