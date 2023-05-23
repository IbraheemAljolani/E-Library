using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Wishlist
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int BooksId { get; set; }

        public virtual Book Books { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
