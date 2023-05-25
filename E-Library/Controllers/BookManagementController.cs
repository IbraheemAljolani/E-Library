
using Dapper;
using E_Library.DTOs;
using E_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks.Dataflow;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookManagementController : ControllerBase
    {
        private readonly ELibraryContext _context;
        private readonly DapperContext _contextdap;
        public BookManagementController(ELibraryContext context, DapperContext _contextdap)
        {
            this._context = context;
            this._contextdap = _contextdap;
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> ViewBook([FromRoute] int id)
        {
            try
            {
                var checkBookId = await _context.Books.SingleOrDefaultAsync(x => x.BookId == id);
                if (checkBookId != null)
                {
                    var getAuther = await _context.Authors.SingleOrDefaultAsync(x => x.AuthorId == checkBookId.AuthorId);
                    ViewBookDTO viewBook = new ViewBookDTO
                    {
                        Title = checkBookId.Title,
                        AuthorName = getAuther.FirstName + " " + getAuther.LastName,
                        Publisher = checkBookId.Publisher,
                        PublicationDate = checkBookId.PublicationDate,
                        Isbn = checkBookId.Isbn,
                        AvailableBooks = checkBookId.AvailableBooks,
                        NumberOfPages = checkBookId.NumberOfPages,
                        Summary = checkBookId.Summary,
                    };
                    return Ok(viewBook);
                }
                return NotFound("The book does not exist");
            }
            catch (Exception ex)
            {
                return Ok($"Error {ex.Message}");
            }

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> SearchBook([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber, [FromQuery] string? authors, [FromQuery] string? titles, [FromQuery] string? isbns, [FromQuery] string? publishers)
        {
            try
            {
                var procedureName = "sp_SearchBook";
                using (var connection = _contextdap.CreateConnection())
                {
                    var addNewBorrow = await connection.QueryAsync(procedureName, new { @author = authors, @title = titles, @isbn = isbns, @publisher = publishers }, commandType: CommandType.StoredProcedure);
                    int skipAmount = pageSize * pageNumber - (pageSize);
                    return Ok(addNewBorrow.Skip(skipAmount).Take(pageSize));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ViewBookRatings([FromQuery] int bookId)
        {
            try
            {
                var Rating = await _context.Ratings.ToListAsync();
                var checkBook = await _context.Books.SingleOrDefaultAsync(x => x.BookId == bookId);
                if (checkBook != null)
                {
                    List<ViewRatingResponseDTO> viewRatingResponse = new List<ViewRatingResponseDTO>();
                    foreach (var r in Rating)
                    {
                        if (r.BookId == bookId)
                        {
                            var RatingList = new ViewRatingResponseDTO
                            {
                                Name = _context.Users.SingleOrDefault(x => x.UserId == r.UserId).FirstName + " " + _context.Users.SingleOrDefault(x => x.UserId == r.UserId).LastName,
                                Feedback = r.Feedback,
                                StarCount = r.StarCount,
                                Date = r.Date,
                            };
                            viewRatingResponse.Add(RatingList);
                        }
                    }
                    return Ok(viewRatingResponse);
                }
                return Ok("The book does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDTO borrowBook)
        {
            try
            {
                var checkAvailableBook = await _context.Books.SingleOrDefaultAsync(x => x.BookId == borrowBook.BookId && x.AvailableBooks > 0);
                if (checkAvailableBook != null)
                {
                    DateTime dueDate = borrowBook.BorrowDate.AddDays(-1) > borrowBook.BorrowDate ? borrowBook.BorrowDate : DateTime.Now.AddDays(3);

                    var procedureName = "BorrowBook";


                    using (var connection = _contextdap.CreateConnection())
                    {
                        var addNewBorrow = await connection.ExecuteAsync(procedureName, new { @userId = borrowBook.userId, @BookId = borrowBook.BookId, @BorrowDate = borrowBook.BorrowDate, @DueDate = dueDate }, commandType: CommandType.StoredProcedure);
                        return Ok(addNewBorrow);
                    }
                }
                return Ok("The book is not available at this time");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDTO returnBook)
        {
            try
            {
                var checkBorrowing = await _context.Borrowings.SingleOrDefaultAsync(x => x.BorrowingId == returnBook.BorrowingId && x.ReturnDate == null);
                if (checkBorrowing != null)
                {
                    var procedureName = "ReturnBook";

                    using (var connection = _contextdap.CreateConnection())
                    {
                        var reBook = await connection.ExecuteAsync(procedureName, new { @BorrowingId = returnBook.BorrowingId, @ReturnDate = returnBook.ReturnDate, @Notes = returnBook.Notes }, commandType: CommandType.StoredProcedure);
                        return Ok(reBook);
                    }
                }
                return Ok("The Borrowing does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RateBook([FromBody] RateBookDTO rateBook)
        {
            try
            {
                var procedureName = "sp_InsertRating";
                using (var connection = _contextdap.CreateConnection())
                {
                    var addRate = await connection.ExecuteAsync(procedureName, new
                    {
                        @feedback = rateBook.Feedback != "NULL" ? rateBook.Feedback : null,
                        @starCount = rateBook.StarCount,
                        @date = DateTime.Now,
                        @userId = rateBook.UserId,
                        @bookId = rateBook.BookId,
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(addRate);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateRateBook([FromBody] UpdateRateBookDTO updateRateBook)
        {
            try
            {
                var procedureName = "sp_UpdateRating";
                using (var connection = _contextdap.CreateConnection())
                {
                    var checkRate = await _context.Ratings.SingleOrDefaultAsync(x => x.RatingId == updateRateBook.RitingId);
                    if (checkRate != null)
                    {
                        var addRate = await connection.ExecuteAsync(procedureName, new
                        {
                            @ratingId = checkRate.RatingId,
                            @feedback = updateRateBook.Feedback != "NULL" ? updateRateBook.Feedback : checkRate.Feedback,
                            @starCount = updateRateBook.StarCount != 0 ? updateRateBook.StarCount : checkRate.StarCount,
                            @date = DateTime.Now,
                            @userId = checkRate.UserId,
                            @bookId = checkRate.BookId,
                        }, commandType: CommandType.StoredProcedure);
                        return Ok(addRate);
                    }
                    return Ok("Rating not found");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteRateBook([FromRoute] int id)
        {
            try
            {
                var procedureName = "sp_UpdateRating";
                using (var connection = _contextdap.CreateConnection())
                {
                    var checkRate = await _context.Ratings.SingleOrDefaultAsync(x => x.RatingId == id);
                    if (checkRate != null)
                    {
                        var addRate = await connection.ExecuteAsync(procedureName, new
                        {
                            @ratingId = id,

                        }, commandType: CommandType.StoredProcedure);
                        return Ok(addRate);
                    }
                    return Ok("Rating not found");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        
    }
}
