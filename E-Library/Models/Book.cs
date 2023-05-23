using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Book
    {
        public Book()
        {
            Borrowings = new HashSet<Borrowing>();
            Ratings = new HashSet<Rating>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public int NumberOfPages { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Isbn { get; set; } = null!;
        public int AvailableBooks { get; set; }
        public int TotalBooksAvailable { get; set; }
        public string Summary { get; set; } = null!;

        public virtual ICollection<Borrowing> Borrowings { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
