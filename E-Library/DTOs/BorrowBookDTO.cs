namespace E_Library.DTOs
{
    public class BorrowBookDTO
    {
        public int userId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
    }
}
