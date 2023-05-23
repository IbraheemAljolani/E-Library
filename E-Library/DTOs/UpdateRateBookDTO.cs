using System.ComponentModel;

namespace E_Library.DTOs
{
    public class UpdateRateBookDTO
    {
        public int RitingId { get; set; }
        [DefaultValue("NULL")]
        public string Feedback { get; set; }
        public int StarCount { get; set; }
    }
}
