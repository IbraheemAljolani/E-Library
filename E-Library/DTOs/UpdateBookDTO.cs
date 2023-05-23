using System.ComponentModel;

namespace E_Library.DTOs
{
    public class UpdateBookDTO
    {
        public int BookId { get; set; }
        [DefaultValue("NULL")]
        public string? Title { get; set; }
        [DefaultValue("NULL")]
        public string? Publisher { get; set; }
        [DefaultValue("NULL")]
        public string? PublicationDate { get; set; }
        [DefaultValue("NULL")]
        public string? Isbn { get; set; }
        [DefaultValue(-1)]
        public int? AvailableBooks { get; set; }
        [DefaultValue(-1)]
        public int? TotalBooksAvailable { get; set; }
        [DefaultValue(-1)]
        public int? NumberOfPages { get; set; }
        [DefaultValue("NULL")]
        public string? Summary { get; set; }
        [DefaultValue(-1)]
        public int? AuthorId { get; set; }
    }
}
