using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class VwBooksDetail
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public string Isbn { get; set; } = null!;
        public int? AvailableBooks { get; set; }
        public int? BorrowedBooks { get; set; }
    }
}
