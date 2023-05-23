using System.ComponentModel;

namespace E_Library.DTOs
{
    public class RateBookDTO
    {
        [DefaultValue("NULL")]
        public string? Feedback { get; set; }
        public int StarCount { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
