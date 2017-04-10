using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Dto;
using Portfolio.API.Filters;
using Portfolio.Data.Context;
using Portfolio.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Portfolio.API.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />

    public class AuthController : Controller
    {
        private PortfolioIdentityContext _context;
        private ILogger<AuthController> _logger;
        private SignInManager<PortfolioIdentityUser> _signInMgr;
        private UserManager<PortfolioIdentityUser> _userMgr;
        private IPasswordHasher<PortfolioIdentityUser> _hasher;
        private IConfigurationRoot _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="signInMgr">The sign in MGR.</param>
        /// <param name="userMgr">The user MGR.</param>
        /// <param name="hasher">The hasher.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="config">The configuration.</param>
        public AuthController(PortfolioIdentityContext context,
        SignInManager<PortfolioIdentityUser> signInMgr,
        UserManager<PortfolioIdentityUser> userMgr,
        IPasswordHasher<PortfolioIdentityUser> hasher,
        ILogger<AuthController> logger,
        IConfigurationRoot config)
        {
            _context = context;
            _signInMgr = signInMgr;
            _logger = logger;
            _userMgr = userMgr;
            _hasher = hasher;
            _config = config;
        }
        
        /// <summary>
        /// Logins the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        [HttpPost("api/auth/login")]
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        public async Task<IActionResult> Login([FromBody] CredentialDto dto)
        {
            try
            {
                var result = await _signInMgr.PasswordSignInAsync(dto.UserName, dto.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in: {ex}");
            }

            return BadRequest("Failed to login");
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="dto">The model.</param>
        /// <returns></returns>
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialDto dto)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(dto.UserName);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userMgr.GetClaimsAsync(user);

                        var claims = new[]
                        {
                          new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                          new Claim(JwtRegisteredClaimNames.FamilyName, user.Surname),
                          new Claim(JwtRegisteredClaimNames.Email, user.Email)
                        }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          issuer: _config["Tokens:Issuer"],
                          audience: _config["Tokens:Audience"],
                          claims: claims,
                          expires: DateTime.UtcNow.AddMinutes(15),
                          signingCredentials: creds
                          );

                        return Ok(new
                        {
                            //Generate a JWT security token
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest("Failed to generate token");
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [ValidateModel, SuppressMessage("Security", "SG0016:Controller method is vulnerable to CSRF")]
        [HttpPost("api/auth/admin")]
        [Authorize(Policy = "SuperUsers")]
        public IActionResult CreateAdmin([FromBody] UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new PortfolioIdentityUser
                    {
                        UserName = user.Username,
                        Email = user.EmailAddress,
                        Name = user.Name,
                        Surname = user.Surname,
                        EmailConfirmed = false,
                        LockoutEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var roleStore = new RoleStore<IdentityRole>(_context);

                    var adminRole = _context.Roles.FirstOrDefault(r => r.Name == "Admin");

                    if (!_context.Users.Any(u => u.UserName == newUser.UserName))
                    {
                        var passwordHasher = new PasswordHasher<PortfolioIdentityUser>();
                        var hashed = passwordHasher.HashPassword(newUser, user.Password);
                        newUser.PasswordHash = hashed;

                        var userStore = new UserStore<PortfolioIdentityUser>(_context);
                        var claim = new Claim("SuperUser", "True");

                        userStore.AddToRoleAsync(newUser, "Admin");
                        userStore.AddClaimsAsync(newUser, new List<Claim> { claim });
                        _context.Users.Add(newUser);

                    }

                    var result = _context.SaveChangesAsync();

                    if (result != null)
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while creating user: {ex}");
            }

            return BadRequest("Failed to login");
        }
    }
    
}