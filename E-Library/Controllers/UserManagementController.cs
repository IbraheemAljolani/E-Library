using E_Library.DTOs;
using E_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly ELibraryContext _context;
        public UserManagementController(ELibraryContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> ViewUserProfile([FromRoute] int id)
        {
            try
            {
                var checkAccount = await _context.Users.SingleOrDefaultAsync(x => x.UserId == id);
                if (checkAccount != null)
                {
                    var checkUserType = _context.Usertypes.SingleOrDefault(x => x.TypeId == checkAccount.TypeId);
                    if (checkUserType != null)
                    {
                        UserResponseDTO userResponse = new UserResponseDTO
                        {
                            FirstName = checkAccount.FirstName,
                            LastName = checkAccount.LastName,
                            Email = checkAccount.Email,
                            PhoneNumber = checkAccount.PhoneNumber,
                            Address = checkAccount.Address,
                            DateOfBirth = checkAccount.DateOfBirth,
                            TypeName = checkUserType.TypeName
                        };
                        return Ok(userResponse);
                    }

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Route("[action]/{userId}/{bookId}")]
        public async Task<IActionResult> AddBooktoWishlist([FromRoute] int userId, [FromRoute] int bookId)
        {
            try
            {
                var checkUser = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userId);
                if (checkUser != null)
                {
                    var checkBook = await _context.Books.SingleOrDefaultAsync(x => x.BookId == bookId);
                    if (checkBook != null)
                    {
                        Wishlist wishlist = new Wishlist
                        {
                            UserId = userId,
                            BooksId = bookId,
                        };
                        await _context.AddAsync(wishlist);
                        await _context.SaveChangesAsync();
                        return Ok("Added");
                    }
                    return Ok("The book does not exist");
                }
                return Ok("The user does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] int id, [FromBody] EditUserProfileDTO editUserProfile)
        {
            try
            {
                if (editUserProfile == null)
                {
                    return BadRequest("The request body is empty.");
                }

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserId == id);
                if (user == null)
                {
                    return NotFound($"User with id {id} not found.");
                }

                if (!string.IsNullOrWhiteSpace(editUserProfile.FirstName) && editUserProfile.FirstName != "string")
                {
                    user.FirstName = editUserProfile.FirstName;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                if (!string.IsNullOrWhiteSpace(editUserProfile.LastName) && editUserProfile.LastName != "string")
                {
                    user.LastName = editUserProfile.LastName;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (!string.IsNullOrWhiteSpace(editUserProfile.Email) && editUserProfile.Email != "user@example.com")
                {
                    user.Email = editUserProfile.Email;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (!string.IsNullOrWhiteSpace(editUserProfile.PhoneNumber) && editUserProfile.PhoneNumber != "string")
                {
                    user.PhoneNumber = editUserProfile.PhoneNumber;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (!string.IsNullOrWhiteSpace(editUserProfile.Address) && editUserProfile.Address != "string")
                {
                    user.Address = editUserProfile.Address;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (editUserProfile.DateOfBirth != null && editUserProfile.DateOfBirth != DateTime.Now.Date)
                {
                    user.DateOfBirth = (DateTime)editUserProfile.DateOfBirth;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                return Ok("User profile updated successfully.");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        
        [HttpDelete]
        [Route("[action]/{userId}/{bookId}")]
        public async Task<IActionResult> RemoveBookfromWishlist([FromRoute] int userId, [FromRoute] int bookId)
        {
            try
            {
                var checkUser = await _context.Users.SingleOrDefaultAsync(x=>x.UserId == userId);
                if(checkUser != null)
                {
                    var checkBook = await _context.Books.SingleOrDefaultAsync(x=>x.BookId == bookId);
                    if (checkBook != null)
                    {
                        var removeWishlist = await _context.Wishlists.SingleOrDefaultAsync(x=>x.UserId == userId && x.BooksId == bookId);
                        if(removeWishlist != null)
                        {
                            _context.Remove(removeWishlist);
                            await _context.SaveChangesAsync();
                            return Ok("Deleted");
                        }
                    }
                }
                return Ok("The book was not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
