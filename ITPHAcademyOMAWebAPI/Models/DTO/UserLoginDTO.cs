using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITPHAcademyOMAWebAPI.Models
{
    public partial class UserLoginDTO
    {


        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
