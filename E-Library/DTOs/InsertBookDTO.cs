using System.ComponentModel.DataAnnotations;

namespace E_Library.DTOs
{
    public class InsertBookDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public int AvailableBooks { get; set; }
        [Required]
        public int TotalBooksAvailable { get; set; }
        [Required]
        public int NumberOfPages { get; set; }
        [Required]
        public string Summary { get; set; }
      
    }
}
