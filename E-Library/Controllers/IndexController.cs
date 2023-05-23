using E_Library.DTOs;
using E_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ELibraryContext _context;
        private readonly IConfiguration _configuration;
        public IndexController(ELibraryContext context,IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
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
                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
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
                        string tokin = CreateToken(checkAccount);
                        Login login = new Login()
                        {
                            Email = register.Email,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            CurrentToken = tokin,
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
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == login.Email);
                if (checkAccount == null)
                {
                    return NotFound("Email does not exist");
                }

                if(!VerifyPasswordHash(login.Password, checkAccount.PasswordHash, checkAccount.PasswordSalt))
                {
                    return BadRequest("Wrong Password");
                }
                
                string tokin = CreateToken(checkAccount);
                checkAccount.LastLogin = DateTime.Now;
                checkAccount.CurrentToken = tokin;
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
                var checkAccount = await _context.Logins.SingleOrDefaultAsync(x => x.Email == changePassword.Email);
                if (!VerifyPasswordHash(changePassword.OldPassword, checkAccount.PasswordHash, checkAccount.PasswordSalt))
                {
                    return BadRequest("Wrong Password");
                }
                if (checkAccount != null)
                {
                    
                    if (changePassword.NewPassword == changePassword.ConfigurePassword)
                    {
                        CreatePasswordHash(changePassword.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                        checkAccount.PasswordHash = passwordHash;
                        checkAccount.PasswordSalt = passwordSalt;
                        await LogoutAccount(checkAccount.LoginId);
                        _context.Update(checkAccount);
                        await _context.SaveChangesAsync();
                        return Ok("The password has been changed successfully");
                    }
                    return Ok("The new password does not match");
                }
                return Ok("Account not found");
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
                        CreatePasswordHash(forgotPassword.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                        checkAccount.PasswordHash = passwordHash;
                        checkAccount.PasswordSalt = passwordSalt;
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
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(Login login)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email , login.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
