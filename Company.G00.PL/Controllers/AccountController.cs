using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G00.PL.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        [HttpGet]  // Get : /Account/SignUp

        public IActionResult SignUp()
        {
            return View();
        }

        //PA$$word123
       // Rokaa$Saleeh123

        [HttpPost]

        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(ModelState.IsValid) // Server Side Validation 
            {

                var User = await _userManager.FindByNameAsync(nameof(model.UserName));

                if(User is null)
                {
                    User = await _userManager.FindByEmailAsync(nameof(model.Email));
                    if(User is null)
                    {

                        User = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };

                        // model.Password => To Make The Hash Process 
                        var Result = await _userManager.CreateAsync(User, model.Password);

                        if (Result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        // For Show Him The Error Details
                        foreach (var Error in Result.Errors)
                        {
                            ModelState.AddModelError("", Error.Description);
                        }
                    }

                    ModelState.AddModelError("", "Invalid Sign Up ");
                }


            }

            return View(model);
        }

        #endregion

        #region SignIn

        [HttpGet]

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
               var User = await _userManager.FindByEmailAsync(model.Email);

                if (User is not null)
                {
                    var flag =  await _userManager.CheckPasswordAsync(User, model.Password);
                    if (flag)
                    {
                        // Sign In 
                         var Result = await  _signInManager.PasswordSignInAsync(User,model.Password,model.RememberMe,false);
                       
                        if(Result.Succeeded)
                        {
                           return RedirectToAction(nameof(HomeController.Index),"Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Login ");

            }
            return View(model);
        }


        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

    }
}
