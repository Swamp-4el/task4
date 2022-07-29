using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task4.Contexts;
using task4.Helpers;
using task4.Models;
using task4.Models.DBModels;

namespace task4.Controllers
{
    [Authorize]
	public class UsersController : Controller
	{
        private readonly Task4Context _dbContext;
        
		public UsersController(Task4Context dbContext)
        {
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			if (!IsUserActive())
				return RedirectToAction("Logout", "Account");

			var model = new UsersViewModel();

			model.Users = await _dbContext.Users
				.Select(u => new UserInfo
				{
					UserName = u.UserName,
					Email = u.Email,
					Status = UserStatus.GetStatus(u.IsBlocked, u.IsDelete),
					IsSelected = false,
                    CreateDate = u.CreateDate,
                    LastLogin = u.LastLogin,
                    Id = u.Id,
				})
				.ToListAsync();

			var allCountUsers = _dbContext.Users.Count();
			var activityCount = model.Users.Where(u => u.Status == UserStatus.Active).Count();
			var blockedCount = model.Users.Where(u => u.Status == UserStatus.Blocked).Count();

			model.ActivityProcent = GetProcent(activityCount, allCountUsers);
			model.BlockedProcent = GetProcent(blockedCount, allCountUsers);
			model.DeleteProcent = GetProcent(allCountUsers - activityCount - blockedCount, allCountUsers);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> BlockUsers(UsersViewModel model)
        {
            return await ActionOnUsers(model, BlockUserAndPossibleDisconnect);
        }

        [HttpPost]
		public async Task<IActionResult> ActivateUsers(UsersViewModel model)
		{
            return await ActionOnUsers(model, ActivateUserAndNotDisconnet);
        }

		[HttpPost]
		public async Task<IActionResult> DeleteUsers(UsersViewModel model)
		{
            return await ActionOnUsers(model, DeleteUserAndPossibleDisconnect);
        }

        private async Task<IActionResult> ActionOnUsers(UsersViewModel model, Func<UserInfo, List<User>, bool> action)
        {
            if (!IsUserActive())
                return RedirectToAction("Logout", "Account");

            var selectedUsers = model.Users.Where(u => u.IsSelected).ToList();
            var users = await _dbContext.Users.ToListAsync();
            var disconnectUser = false;

            foreach (var user in selectedUsers)
            {
                if (action(user, users))
                    disconnectUser = true;
            }

            await _dbContext.SaveChangesAsync();

            if (disconnectUser)
                return RedirectToAction("Logout", "Account");

            return RedirectToAction("Index", "Users");
        }

        private bool BlockUserAndPossibleDisconnect(UserInfo user, List<User> users)
        {
            var disconnectUser = false;
            var currentUser = users.FirstOrDefault(u =>
                    u.Id == user.Id &&
                    !u.IsDelete);

            if (currentUser != null)
            {
                currentUser.IsBlocked = true;

                if (user.UserName == User.Identity.Name)
                    disconnectUser = true;
            }

            return disconnectUser;
        }

        private bool ActivateUserAndNotDisconnet(UserInfo user, List<User> users)
        {
            var currentUser = users.FirstOrDefault(u =>
                    u.Id == user.Id &&
                    !u.IsDelete);

            if (currentUser != null)
                currentUser.IsBlocked = false;

            return false;
        }

        private bool DeleteUserAndPossibleDisconnect(UserInfo user, List<User> users)
        {
            var disconnectUser = false;
            var currentUser = users.FirstOrDefault(u =>
                    u.Id == user.Id &&
                    !u.IsDelete);

            if (currentUser != null)
            {
                currentUser.IsDelete = true;
                currentUser.IsBlocked = false;

                if (user.UserName == User.Identity.Name)
                    disconnectUser = true;
            }

            return disconnectUser;
        }

        private bool IsUserActive()
		{
			var user = _dbContext.Users
				.FirstOrDefault(u => u.UserName == User.Identity.Name &&
				!u.IsBlocked &&
				!u.IsDelete);

			return !(user is null);
		}

		private string GetProcent(float usersCount, float allUsersCount)
        {
			var maxProcent = 100f;

			return $"{usersCount / allUsersCount * maxProcent}%".Replace(',', '.');
		}
	}
}