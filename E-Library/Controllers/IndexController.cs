using E_Library.DTOs;
using E_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ELibraryContext _context;
        public IndexController(ELibraryContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountDTO register)
        {
            try
            {
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == register.Email);
                if (checkAccount == null)
                {
                    User user = new User()
                    {
                        TypeId = 2,
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        Address = register.Address,
                        DateOfBirth = register.DateOfBirth,
                    };
                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var checkUser = _context.Users.SingleOrDefault(x => x.Email == register.Email);
                    if (checkUser != null)
                    {
                        Login login = new Login()
                        {
                            Email = register.Email,
                            Password = register.Password,
                            UserId = checkUser.UserId
                        };
                        await _context.AddAsync(login);
                        await _context.SaveChangesAsync();
                        return Ok("Account has been successfully registered");
                    }

                }
                return Ok("The email is already registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> LoginAccount([FromBody] LoginAccountDTO login)
        {
            try
            {
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == login.Email && x.Password == login.Password);
                if (checkAccount != null)
                {
                    checkAccount.LastLogin = DateTime.Now;
                    _context.Update(checkAccount);
                    await _context.SaveChangesAsync();
                    var user = await _context.Users.SingleOrDefaultAsync(x => x.UserId == checkAccount.UserId);
                    LoginResponseDTO loginResponse = new LoginResponseDTO()
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        TypeName = _context.Usertypes.FirstOrDefault(x => x.TypeId == user.TypeId).TypeName,
                    };
                    return Ok(loginResponse);
                }
                return Ok("Account not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<IActionResult> LogoutAccount([FromRoute] int id)
        {
            try
            {
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.LoginId == id);
                if (checkAccount != null)
                {
                    checkAccount.LastLogout = DateTime.Now;
                    _context.Update(checkAccount);
                    await _context.SaveChangesAsync();
                    return Ok("Logout succeeded");
                }
                return Ok("An error occurred, please try again");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePassword)
        {
            try
            {
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == changePassword.Email && x.Password == changePassword.OldPassword);
                if (checkAccount != null)
                {
                    if(changePassword.NewPassword == changePassword.ConfigurePassword)
                    {
                        checkAccount.Password = changePassword.NewPassword;
                        _context.Update(checkAccount);
                        await _context.SaveChangesAsync();
                        return Ok("The password has been changed successfully");
                    }
                    return Ok("The new password does not match");
                }
                return Ok("The old password is incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword )
        {
            try
            {
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == forgotPassword.Email);
                if (checkAccount != null)
                {
                    if (forgotPassword.NewPassword == forgotPassword.ConfigurePassword)
                    {
                        checkAccount.Password = forgotPassword.NewPassword;
                        _context.Update(checkAccount);
                        await _context.SaveChangesAsync();
                        return Ok("The password has been set successfully");
                    }
                    return Ok("The new password does not match");
                }
                return Ok("The operation did not work, please try again");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
