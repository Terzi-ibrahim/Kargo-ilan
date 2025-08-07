using Kargo_İlan.DTOs.Account;
using Kargo_İlan.DTOs.Company;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kargo_İlan.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        
        public CompanyController(ICompanyService company)
        {
         _companyService = company;
        }
        private int GetLoggedInUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out var loggedInUserId))
            {
                return loggedInUserId;
            }
            else
            {
                throw new UnauthorizedAccessException("Giriş yapan kullanıcı bilgisi alınamadı.");
            }
        }


        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
          
            var companyUsers = await _companyService.GetCompanyUsersAsync();

           
            if (companyUsers==null)
            {
                ViewBag.Message = "Şirket kullanıcısı bulunamadı.";
                return View(new List<User>());
            }

            return View(companyUsers);
        }
        [HttpGet]
        [Authorize(Roles = "Company")]
        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> RegisterCompany(RegisterCompany model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var (success, errorMessage) = await _companyService.RegisterCompanyAsync(model, userId);
            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Hata oluştu.");
                return View(model);
            }
            TempData["SuccessMessage"] = "Şirket kaydınız başarıyla tamamlandı!";
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> CompanyProfile()
        {
            var userId = GetLoggedInUserId();
            var companyProfile = await _companyService.GetCompanyProfileByUserIdAsync(userId);
            if (companyProfile == null)
            {
                TempData["ErrorMessage"] = "Şirket bilgisi bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            return View(companyProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompanyProfile(CompanyProfile model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = GetLoggedInUserId();
        
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _companyService.UpdateCompanyProfileAsync(userId, model);

            if (!result)
            {
                TempData["ErrorMessage"] = "Şirket bilgileri güncellenemedi.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Şirket bilgileri başarıyla güncellendi.";
            return RedirectToAction("CompanyProfile");
        }
        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> DetailCompanyUser(int id)
        {
            var model = await _companyService.GetUserDetailAsync(id);
            if (model == null)
                return NotFound(); // Eğer model null ise 404 döner

            return View("UserCompany/DetailCompanyUser", model);
        }
     
        [Authorize(Roles = "Company")]
        [HttpGet]
        public IActionResult AddCompanyUser()
        {
            return View("UserCompany/CreateCompanyUser");


        }
        [Authorize(Roles = "Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompanyUser(AddCompanyUser model)
        {
            if (!ModelState.IsValid)
            {            
         
                return View("UserCompany/CreateCompanyUser", model);

            }

            int loggedInUserId;
            try
            {
                loggedInUserId = GetLoggedInUserId();
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }


            var (success, errorMessage) = await _companyService.AddCompanyUserAsync(model, loggedInUserId);

            if (!success)
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("AddCompanyUser", "Company");
            }



            TempData["SuccessMessage"] = "Şirket kullanıcısı başarıyla eklendi!";
            return RedirectToAction("CompanyProfile", "Company");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCompanyUser(int userCompanyId, int companyUserId)
        {
            // Kullanıcı bilgilerini alıyoruz
            var result = await _companyService.GetCompanyUserByIdAsync(companyUserId);

            // Kullanıcı bulunamazsa hata mesajı ile yönlendiriyoruz
            if (result == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("CompanyStaffList", "Company");
            }

            // Kullanıcıyı model olarak view'a gönderiyoruz
            var model = new CompanyUserDto
            {
                User_id = result.User_id,
                Company_id = result.Company_id,
                Name = result.Name,
                SurName = result.SurName,
                Email = result.Email,
                Phone = result.Phone,
                IsDeleted = result.IsDeleted
            };

            return View("UserCompany/DeleteCompanyUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteCompanyUser(int userCompanyId, int companyUserId)
        {
            bool success = await _companyService.DeleteCompanyUserAsync(userCompanyId, companyUserId);

            if (success)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı aktif değil veya bulunamadı.";
            }

            return RedirectToAction("CompanyStaffList", "Company");
        }


        [Authorize(Roles = "Company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompany()
        {
            int loggedInUserId;
            try
            {
               
                loggedInUserId = GetLoggedInUserId();
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("CompanyProfile");
            }


            var (success, message) = await _companyService.DeleteCompanyAsync(loggedInUserId, loggedInUserId);

            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("CompanyProfile");
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index", "Home");
        }

    }
}
