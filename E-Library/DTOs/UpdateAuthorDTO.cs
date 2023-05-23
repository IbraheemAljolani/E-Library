using E_Library.Models;
using System.ComponentModel;

namespace E_Library.DTOs
{
    public class UpdateAuthorDTO
    {
        public int authorId { get; set; }
        [DefaultValue("NULL")]
        public string? firstName { get; set; }
        [DefaultValue("NULL")]
        public string? lastName { get; set; }
        [DefaultValue("NULL")]
        public string? nationality { get; set; }
        [DefaultValue("NULL")]
        public string? dateOfBirth { get; set; }
        [DefaultValue("NULL")]
        public string? dateOfDeath { get; set; }
        [DefaultValue("NULL")]
        public string? biography { get; set; }
    }
}
