using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogout { get; set; }
        public string? CurrentToken { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
