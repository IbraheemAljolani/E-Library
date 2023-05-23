using System.ComponentModel.DataAnnotations;

namespace E_Library.DTOs
{
    public class LoginAccountDTO
    {
        [Required, EmailAddress, RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")]
        public string Email { get; set; } = null!;
        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_]).{8,}$")]
        public string Password { get; set; } = null!;
    }
}
