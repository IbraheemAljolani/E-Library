using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class Usertype
    {
        public Usertype()
        {
            Users = new HashSet<User>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
