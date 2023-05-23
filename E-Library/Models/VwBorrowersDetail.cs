﻿using System;
using System.Collections.Generic;

namespace E_Library.Models
{
    public partial class VwBorrowersDetail
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int? BorrowedBooks { get; set; }
    }
}
