using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class VwActiveBorrowing
    {
        public string BorrowerName { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}
