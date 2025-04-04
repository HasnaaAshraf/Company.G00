using Company.G00.DAL.Models;
using Company.G00.DAL.Sms;
using Company.G00.PL.Dtos;
using Company.G00.PL.Helpers;
using Company.G00.PL.InterfacesHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G00.PL.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioServices _twilioServices;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, ITwilioServices twilioServices = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioServices = twilioServices;
        }

        #region SignUp

        [HttpGet]  // Get : /Account/SignUp

        public IActionResult SignUp()
        {
            return View();
        }

        //PA$$word123
        // Rokaa$Saleeh123
        // nilixex142@motivue.com (password(Saya&&321))

        // Twilio Pass => Hasoo@13579 (Recovery Code [PAV7GXA5UTVXH3NYYCRHEQTC])
        // Twilio Pass => Samaa@ashraf113 

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

        #region Forget Password 

        // Be Sure That Mail Right

        // nilixex142@motivue.com (password(Saya&&321))

        [HttpGet]

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SendResentPasswordUrl(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
               var user = await  _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {

                    // Generate Token 
                     var Token = await _userManager.GeneratePasswordResetTokenAsync(user);


                    // Create URL 
                    // https://localhost:44314/Account/ResetPassword

                       var url = Url.Action("ResetPassword", "Account", new {model.Email , Token},Request.Scheme);

                    // Create Email 

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    // Send Email 

                    // Old Way :
                    //var flag = EmailSetting.SendEmail(email);

                    // New Way :

                    _mailService.SendEmail(email);

                    // Send Check Your Inbox 
                    return RedirectToAction("CheckYourInbox");
                    
                }

                ModelState.AddModelError("", "Invalid Resert Password ");

            }

            return View("ForgetPassword" , model);
        }



        [HttpPost]

        public async Task<IActionResult> SendResentPasswordSms(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                    // Generate Token 
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(user);


                    // Create URL 
                    // https://localhost:44314/Account/ResetPassword

                    var url = Url.Action("ResetPassword", "Account", new { model.Email, Token }, Request.Scheme);

                    // Create Sms 

                    var sms = new Sms()
                    {
                        To = user.PhoneNumber,
                        Body = url
                    };

                    // Send Email 

                    // Old Way :
                    //var flag = EmailSetting.SendEmail(email);

                    // New Way :

                    _twilioServices.SendSms(sms);

                    // Send Check Your Phone 
                    return RedirectToAction("CheckYourPhone");

                }

                ModelState.AddModelError("", "Invalid Resert Password ");

            }

            return View( model);
        }



        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult CheckYourPhone()
        {
            return View();
        }

        #endregion

        #region Reset Password 

        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
           if(ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Null Email Or Token");

                var user = await _userManager.FindByEmailAsync(email);

                if(user is not null)
                {
                   var Result = await _userManager.ResetPasswordAsync(user , token , model.NewPassword);
                   if(Result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                
                }

                ModelState.AddModelError("" , "Invalid Reset Password Operator , Please Try Again ");

            }
            return View(model);
        }


        #endregion


    }
}
