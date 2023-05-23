using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string Biography { get; set; } = null!;
    }
}
