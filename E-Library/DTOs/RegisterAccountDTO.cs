using System.ComponentModel.DataAnnotations;

namespace E_Library.DTOs
{
    public class RegisterAccountDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress, RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")]
        public string Email { get; set; } = null!;
        [Required,Phone,RegularExpression(@"^07\d{8}$")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required,RegularExpression(@"^(?=.*[A-Z])\S(?=.*[0-9])(?=.*[\W_]).{8,}$")]
        public string Password { get; set; } = null!;
    }
}
