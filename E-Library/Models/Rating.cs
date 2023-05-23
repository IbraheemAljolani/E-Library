using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public string? Feedback { get; set; }
        public int StarCount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
