namespace E_Library.DTOs
{
    public class ViewBookDTO
    {
        public string Title { get; set; } = null!;
        public string AuthorName { get; set; }
        public string Publisher { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public string Isbn { get; set; } = null!;
        public int AvailableBooks { get; set; }
        public int NumberOfPages { get; set; }
        public string Summary { get; set; } = null!;
    }
}
