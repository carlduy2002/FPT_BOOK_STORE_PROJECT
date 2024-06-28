using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Book_Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<Accounts> _userManager;

        public AccountController(ApplicationDbContext db,  UserManager<Accounts> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            List<Accounts> account = (List<Accounts>) await _userManager.GetUsersInRoleAsync("Owner");

            List<Accounts> existingAccount = account.OrderByDescending(a => a.Account_Deleted).ThenBy(a => a.UserName).ToList();
    
            return View(existingAccount);
        }

        public async Task<IActionResult> Delete(string id)
        {
            
            Accounts user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                TempData["account-error-message"] = "Error! Account cannot be deleted!";

                return RedirectToAction("Index");
            }
            
            user.Account_Deleted = Status.Deleted.ToString();

            _db.Accounts.Update(user);          

            await _db.SaveChangesAsync();

            TempData["account-message"] = "Account " + user.UserName + " has been successfully deleted!";             

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activate(string id)
        {

            Accounts user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {

                TempData["account-error-message"] = "Error! Account cannot be activated!";

                return RedirectToAction("Index");
            }

            user.Account_Deleted = Status.Existing.ToString();

            _db.Accounts.Update(user);

            await _db.SaveChangesAsync();

            TempData["account-message"] = "Account " + user.UserName + " has been successfully activated!";

            return RedirectToAction("Index");
        }
      

    }
}