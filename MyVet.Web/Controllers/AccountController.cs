using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using MyVet.Web.Helpers;
using MyVet.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyVet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            IUserHelper userHelper,
            IConfiguration configuration,
            DataContext context,
            IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _context = context;
            _mailHelper = mailHelper;
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            return View();
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var owner = await _context.Owners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));
            if (owner == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = owner.User.Address,
                Document = owner.User.Document,
                FirstName = owner.User.FirstName,
                Id = owner.Id,
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var owner = await _context.Owners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                owner.User.Document = model.Document;
                owner.User.FirstName = model.FirstName;
                owner.User.LastName = model.LastName;
                owner.User.Address = model.Address;
                owner.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(owner.User);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                var owner = new Owner
                {
                    Pets = new List<Pet>(),
                    User = user,
                };

                _context.Owners.Add(owner);
                await _context.SaveChangesAsync();

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Username, "Confirmación de email", $"<h1>Confirmación de email</h1>" +
                    $"Para activar el usuario, " +
                    $"por favor da clic en este link:</br></br><a href = \"{tokenLink}\">Confirmación de email</a>");
                ViewBag.Message = "Las instrucciones para activar el usuario han sido enviadas a tu correo.";
                return View(model);
            }
            return View(model);
        }

        private async Task<User> AddUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            var newUser = await _userHelper.GetUserByEmailAsync(model.Username);
            await _userHelper.AddUserToRoleAsync(newUser, "Customer");
            return newUser;
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));//encriptar la llave
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMonths(4),//expiracion del token en 4 meses
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }
            return BadRequest();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrecta.");
            model.Username = string.Empty;
            model.Password = string.Empty;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}