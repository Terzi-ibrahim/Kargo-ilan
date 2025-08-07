using Kargo_İlan.DTOs.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Kargo_İlan.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Kargo_İlan.Controllers
{

    public class AccountController : Controller
    {        
        private readonly IAccountService _accountService;
        public AccountController(IAccountService account)
        {
           
            _accountService = account;
        }
        private int GetUserId()
        {
            return Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {           
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Login model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, errorMessage, principal) = await _accountService.LoginAsync(model);

            if (!success || principal == null)
            {
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);


            var username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "";
            TempData["SuccessMessage"] = $"Giriş başarılı, hoşgeldiniz {username}!";

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, errorMessage) = await _accountService.RegisterAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Kayıt başarısız.");
                return View(model);
            }
            TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                bool success = await _accountService.HandleForgotPasswordAsync(model.Email);

                if (!success)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresine ait bir kullanıcı bulunamadı.");
                    return View(model);
                }
                TempData["SuccessMessage"] = "Parola sıfırlama bağlantısı e-posta adresinize gönderildi.";
                return RedirectToAction("Index", "Home"); 
            }

            return View(model);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangePassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home");  
            }

            var isValidToken = _accountService.ValidateResetTokenAsync(token).Result;

            if (!isValidToken)
            {
                return RedirectToAction("Error", "Home");  
            }

            var model = new ChangePassword
            {
                Token = token
            };

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var isSuccess = await _accountService.ChangePasswordAsync(model);

            if (!isSuccess)
            {
                return RedirectToAction("Error", "Home");  
            }
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }         
            var model = await _accountService.GetProfileAsync(userId);

            if (model == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([Bind(Prefix = "Profile")] Profile profileDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Profil bilgileri geçerli değil.";
                return RedirectToAction("Profile");
            }

            int userId = GetUserId();  

       
            var success = await _accountService.UpdateProfileAsync(userId, profileDto);

            if (!success)
            {
                TempData["ErrorMessage"] = "Profil güncelleme başarısız.";
                return RedirectToAction("Profile"); 
            }

            TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
            return RedirectToAction("Profile");  
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserChangePassword([Bind(Prefix = "UserChangePassword")] UserChangePassword pwdDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Şifre bilgileri geçerli değil.";
                var vm = await _accountService.BuildProfileViewModelAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
                return View("Profile", vm);
            }

            int userId = GetUserId();
            var changePasswordSuccess = await _accountService.ChangePasswordAsync(userId, pwdDto.CurrentPassword, pwdDto.Password);

            if (!changePasswordSuccess)
            {
                TempData["ErrorMessage"] = "Mevcut şifre yanlış.";               
                var vm = await _accountService.BuildProfileViewModelAsync(userId);
                return View("Profile", vm);
            }

            if (pwdDto.Password != pwdDto.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Şifreler uyuşmuyor.";
                var vm = await _accountService.BuildProfileViewModelAsync(userId);
                return View("Profile", vm);
            }


            TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi.";
            return RedirectToAction("Profile");
        }
     
    }
}
