using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_Library.DTOs
{
    public class EditUserProfileDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress, RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
    }
}
