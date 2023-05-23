using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class User
    {
        public User()
        {
            Borrowings = new HashSet<Borrowing>();
            Logins = new HashSet<Login>();
            Ratings = new HashSet<Rating>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int TypeId { get; set; }

        public virtual Usertype Type { get; set; } = null!;
        public virtual ICollection<Borrowing> Borrowings { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
