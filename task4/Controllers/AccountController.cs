using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using task4.Contexts;
using task4.Models;
using task4.Models.DBModels;
using task4.Services;

namespace task4.Controllers
{
	public class AccountController : Controller
	{
		private readonly Task4Context _dbContext;

		private readonly IHashingService _hashingService;

		public AccountController(Task4Context dbContext, IHashingService hashingService)
		{
			_dbContext = dbContext;
			_hashingService = hashingService;
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated) RedirectToAction("Index", "Users");

			return View();
		}

		[HttpGet]
		public IActionResult Registration()
		{
			if (User.Identity.IsAuthenticated) RedirectToAction("Index", "Users");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var hashPassword = _hashingService.GetHashString(model.Password);

			var user = _dbContext.Users.FirstOrDefault(u =>
				u.UserName == model.UserName &&
				!u.IsDelete);

			if (user != null && user.Password == hashPassword && !user.IsBlocked)
			{
				await Authenticate(model.UserName);
                user.LastLogin = DateTime.Now;

				return RedirectToAction("Index", "Users");
			}

            AddModelState(user, hashPassword);

            return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Registration(RegistrationViewModel model)
		{
			if (!ModelState.IsValid) 
                return View(model);

			var user = _dbContext.Users
				.FirstOrDefault(u => u.UserName == model.UserName && !u.IsDelete);

			if (user is null)
			{
                await RegistrationUser(model);

				return RedirectToAction("Index", "Users");
			}
			
			ModelState.AddModelError("", "Such a user is already registered");

			return View(model);
		}

        public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Login", "Account");
		}

		private async Task Authenticate(string userName)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};

			var id = new ClaimsIdentity(claims,
										"ApplicationCookie",
										ClaimsIdentity.DefaultNameClaimType,
										ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
										  new ClaimsPrincipal(id));
        }

        private async Task RegistrationUser(RegistrationViewModel model)
        {
            _dbContext.Users.Add(new User
            {
                UserName = model.UserName,
                Password = _hashingService.GetHashString(model.Password),
                Email = model.Email,
                CreateDate = DateTime.Now,
                LastLogin = DateTime.Now,
                IsBlocked = false,
                IsDelete = false,
            });

            _dbContext.SaveChanges();
            await Authenticate(model.UserName);
        }

        private void AddModelState(User user, string password)
        {
            if (user is null)
                ModelState.AddModelError("", "There is no such user!");
            else if (user.Password != password)
                ModelState.AddModelError("", "Invalid password!");
            else if (user.IsBlocked)
                ModelState.AddModelError("", "The user has been blocked!");
        }
	}
}
