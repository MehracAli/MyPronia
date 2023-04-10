using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Entities.UserModels;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM account)
        {
            if(!ModelState.IsValid) return View();
            if(!account.Terms) return View();
            User user = new()
            {
                FullName = string.Concat(account.Firstname," ",account.Surname),
                Email = account.Email,
                UserName = account.Username,
            };
            IdentityResult result = await _userManager.CreateAsync(user, account.Password); 
            if(!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM account)
        {
            if (!ModelState.IsValid) return View();

            User user = await _userManager.FindByNameAsync(account.Username);

            if(user is null)
            {
                ModelState.AddModelError("", "Incorrect Username or Password");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
                    .PasswordSignInAsync(user, account.Password, account.RememberMe, true);
            if(!result.Succeeded)
            {
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError("","You can try to login after 5 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Incorrect Username or Password");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Show()
        {
            return Json(HttpContext.User.Identity.IsAuthenticated);
        }
    }
}
