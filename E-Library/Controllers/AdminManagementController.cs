using Dapper;
using E_Library.DTOs;
using E_Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminManagementController : ControllerBase
    {
        private readonly ELibraryContext _context;
        private readonly DapperContext _contextdap;
        public AdminManagementController(ELibraryContext context, DapperContext _contextdap)
        {
            this._context = context;
            this._contextdap = _contextdap;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ViewBorrowedBooks([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber, [FromQuery] string? name, [FromQuery, EmailAddress] string? email, [FromQuery, RegularExpression(@"^07\d{8}$")] string? phone)
        {
            try
            {
                var query = _context.VwActiveBorrowings.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.BorrowerName.Contains(name));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(x => x.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(x => x.PhoneNumber.Contains(phone));
                }

                int skipAmount = pageSize * pageNumber - (pageSize);
                return Ok(query.Skip(skipAmount).Take(pageSize));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ViewUserList([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber, [FromQuery] string? name, [FromQuery, EmailAddress] string? email, [FromQuery, RegularExpression(@"^07\d{8}$")] string? phone)
        {
            try
            {
                var query = _context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.FirstName.Contains(name) || x.LastName.Contains(name));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(x => x.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(x => x.PhoneNumber.Contains(phone));
                }
                List<ViewUserListDTO> viewUserLists = query.Select(item => new ViewUserListDTO
                {
                    FullName = item.FirstName + " " + item.LastName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    DateOfBirth = item.DateOfBirth.Date
                }).ToList();

                int skipAmount = pageSize * pageNumber - (pageSize);
                return Ok(viewUserLists.Skip(skipAmount).Take(pageSize));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddBook([FromBody] InsertBookDTO insertBook)
        {
            try
            {
                var checkAuther = await _context.Authors.SingleOrDefaultAsync(x => (x.FirstName.Contains(insertBook.AuthorName) + " " + x.LastName.Contains(insertBook.AuthorName)) == insertBook.AuthorName || x.FirstName == insertBook.AuthorName
                || x.LastName == insertBook.AuthorName);
                if (checkAuther == null)
                {
                    return Ok("The author does not exist. Please register the author first");
                }
                var ckeckBook = await _context.Books.SingleOrDefaultAsync(x => x.Title == insertBook.Title);
                if (ckeckBook == null)
                {
                    Book book = new Book
                    {
                        Title = insertBook.Title,
                        AuthorId = checkAuther.AuthorId,
                        Publisher = insertBook.Publisher,
                        PublicationDate = insertBook.PublicationDate.Date,
                        Isbn = insertBook.Isbn,
                        AvailableBooks = insertBook.AvailableBooks,
                        TotalBooksAvailable = insertBook.TotalBooksAvailable,
                        NumberOfPages = insertBook.NumberOfPages,
                        Summary = insertBook.Summary,
                    };
                    await _context.AddAsync(book);
                    await _context.SaveChangesAsync();
                    return Ok("The book has been added successfully");
                }
                return Ok("The book already exists");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddAuthor([FromBody] InsertAuthorDTO insertAuthor)
        {
            try
            {
                var checkAuthor = await _context.Authors.SingleOrDefaultAsync(x => x.FirstName == insertAuthor.FirstName && x.LastName == insertAuthor.LastName);
                if (checkAuthor == null)
                {
                    var procedureName = "sp_InsertAuthor";
                    DateTime? DateOfDeath;
                    if (insertAuthor.DateOfDeath == "NULL")
                    {
                        DateOfDeath = null;
                    }
                    else
                    {
                        DateOfDeath = DateTime.Parse(insertAuthor.DateOfDeath);

                    }
                    using (var connection = _contextdap.CreateConnection())
                    {
                        var addNewAuthor = await connection.ExecuteAsync(procedureName, new { @firstName = insertAuthor.FirstName, @lastName = insertAuthor.LastName, @nationality = insertAuthor.Nationality, @dateOfBirth = insertAuthor.DateOfBirth, @dateOfDeath = DateOfDeath, @biography = insertAuthor.Biography }, commandType: CommandType.StoredProcedure);
                        return Ok(addNewAuthor);
                    }
                }
                return Ok("The author already exists");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorDTO updateAuthor)
        {
            try
            {
                var oldAuther = await _context.Authors.SingleOrDefaultAsync(x => x.AuthorId == updateAuthor.authorId);
                DateTime? DateOfDeath;
                DateTime? DateOfBirth;
                if (updateAuthor.dateOfDeath == "NULL")
                {
                    DateOfDeath = oldAuther.DateOfDeath;
                }
                else
                {
                    DateOfDeath = DateTime.Parse(updateAuthor.dateOfDeath);
                }
                if (updateAuthor.dateOfBirth == "NULL")
                {
                    DateOfBirth = oldAuther.DateOfBirth;
                }
                else
                {
                    DateOfBirth = DateTime.Parse(updateAuthor.dateOfBirth);
                }

                var procedureName = "sp_UpdateAuthor";
                using (var connection = _contextdap.CreateConnection())
                {
                    var updateAurhers = await connection.ExecuteAsync(procedureName, new
                    {
                        @authorId = updateAuthor.authorId,
                        @firstName = updateAuthor.firstName != "NULL" ? updateAuthor.firstName : oldAuther.FirstName,
                        @lastName = updateAuthor.lastName != "NULL" ? updateAuthor.lastName : oldAuther.LastName,
                        @nationality = updateAuthor.nationality != "NULL" ? updateAuthor.nationality : oldAuther.Nationality,
                        @dateOfBirth = DateOfBirth,
                        @dateOfDeath = DateOfDeath,
                        @biography = updateAuthor.biography != "NULL" ? updateAuthor.biography : oldAuther.Biography
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(updateAurhers);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDTO updateBook)
        {
            try
            {
                var oldBook = await _context.Books.SingleOrDefaultAsync(x => x.BookId == updateBook.BookId);
                DateTime? PublicationDate;
                if (updateBook.PublicationDate == "NULL")
                {
                    PublicationDate = oldBook.PublicationDate;
                }
                else
                {
                    PublicationDate = DateTime.Parse(updateBook.PublicationDate);
                }


                var procedureName = "sp_UpdateBook";
                using (var connection = _contextdap.CreateConnection())
                {
                    var updateBooks = await connection.ExecuteAsync(procedureName, new
                    {
                        @bookId = updateBook.BookId,
                        @title = updateBook.Title != "NULL" ? updateBook.Title : oldBook.Title,
                        @authorId = updateBook.AuthorId != -1 ? updateBook.AuthorId : oldBook.AuthorId,
                        @publisher = updateBook.Publisher != "NULL" ? updateBook.Publisher : oldBook.Publisher,
                        @publicationDate = PublicationDate,
                        @isbn = updateBook.Isbn != "NULL" ? updateBook.Isbn : oldBook.Isbn,
                        @availableBooks = updateBook.AvailableBooks != -1 ? updateBook.AvailableBooks : oldBook.AvailableBooks,
                        @totalBooksAvailable = updateBook.TotalBooksAvailable != -1 ? updateBook.TotalBooksAvailable : oldBook.TotalBooksAvailable,
                        @numberOfPages = updateBook.NumberOfPages != -1 ? updateBook.NumberOfPages : oldBook.NumberOfPages,
                        @summary = updateBook.Summary != "NULL" ? updateBook.Summary : oldBook.Summary,
                    }, commandType: CommandType.StoredProcedure);
                    return Ok(updateBooks);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var checkUser = await _context.Logins.SingleOrDefaultAsync(x => x.LoginId == id);
                if (checkUser != null)
                {
                    _context.Remove(checkUser);
                    await _context.SaveChangesAsync();
                    return Ok("The account has been deleted");
                }
                return Ok("Account not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            try
            {
                var checkBook = await _context.Books.SingleOrDefaultAsync(x => x.BookId == id);
                if (checkBook != null)
                {
                    _context.Remove(checkBook);
                    await _context.SaveChangesAsync();
                    return Ok("The book has been deleted");
                }
                return Ok("The book does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            try
            {
                var checkAuther = await _context.Authors.SingleOrDefaultAsync(x => x.AuthorId == id);
                if (checkAuther != null)
                {
                    _context.Remove(checkAuther);
                    await _context.SaveChangesAsync();
                    return Ok("Author removed");
                }
                return Ok("Author not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
