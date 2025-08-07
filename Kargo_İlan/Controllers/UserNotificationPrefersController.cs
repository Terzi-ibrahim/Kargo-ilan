using Kargo_İlan.DTOs.UserNotificationPrefs;
using Kargo_İlan.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Kargo_İlan.Controllers
{
    public class UserNotificationPrefersController : Controller
    {

        private readonly IUserNotificationPrefsService _userNotificationPrefsService;

        public UserNotificationPrefersController(IUserNotificationPrefsService userNotificationPrefsService)
        {
            _userNotificationPrefsService = userNotificationPrefsService;
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetLoggedInUserId();
            var prefs = await _userNotificationPrefsService.GetByUserIdAsync(userId);

            var model = new List<UserNotificationPrefDto>();
            foreach (var p in prefs)
            {
                var notifications = await _userNotificationPrefsService.GetNotificationsByPreferenceAsync(p);
                model.Add(new UserNotificationPrefDto
                {
                    Id = p.UserNotificationPrefId,
                    YuklemeilId = p.Yuklemeil_id,
                    YuklemeIlName = p.Yukleme_il.ProvinceName,
                    YuklemeilceId = p.Yuklemeilce_id,
                    YuklemeIlceName = p.Yukleme_ilce?.DistrictName,
                    VarisilId = p.Varisil_id,
                    VarisIlName = p.Varis_il.ProvinceName,
                    VarisilceId = p.Varisilce_id,
                    VarisIlceName = p.Varis_ilce?.DistrictName,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    Notifications = notifications
                });
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> ToggleNotificationPrefStatus(int id)
        {
            var userId = GetLoggedInUserId();

            try
            {
                await _userNotificationPrefsService.ToggleActiveStatusAsync(id);
                TempData["SuccessMessage"] = "Bildirim tercihi güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CreateUserNotificationPref()
        {
            var model = await _userNotificationPrefsService.GetCreateViewModelAsync();

            ViewBag.ProvinceList = model.Provinces
                .Select(p => new SelectListItem
                {
                    Value = p.Province_id.ToString(),
                    Text = p.ProvinceName
                }).ToList();

            ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                model.Districts.ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Value.Select(d => new { Id = d.District_id, Name = d.DistrictName })
                )
            );

           

            return View(model.UserNotificationPref); 
        }


        [HttpPost]
        public async Task<IActionResult> CreateUserNotificationPref(UserNotificationPrefCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
              
                var model = await _userNotificationPrefsService.GetCreateViewModelAsync();
                ViewBag.ProvinceList = model.Provinces
                    .Select(p => new SelectListItem
                    {
                        Value = p.Province_id.ToString(),
                        Text = p.ProvinceName
                    }).ToList();

                ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                    model.Districts.ToDictionary(
                        g => g.Key.ToString(),
                        g => g.Value.Select(d => new { Id = d.District_id, Name = d.DistrictName })
                    )
                );

               
                return View(dto);
            }

            int userId = GetLoggedInUserId();
            await _userNotificationPrefsService.CreateAsync(dto, userId);

            TempData["SuccessMessage"] = "Bildirim Tercihi Oluşturuldu.";
            return RedirectToAction("Index");
        } 
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userNotificationPrefsService.GetEditViewModelAsync(id);
            if (model == null) return NotFound();

            ViewBag.Province = model.Provinces.Select(p => new SelectListItem
            {
                Value = p.Province_id.ToString(),
                Text = p.ProvinceName
            }).ToList();

            ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                model.Districts.ToDictionary(
                    g => g.Key.ToString(),
                    g => g.Value.Select(d => new { Id = d.Id, Name = d.Name })
                )
            );

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind(Prefix = "UserNotificationPref")] UserNotificationPrefUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var vm = new UserNotificationPrefUpdateViewModel
                {
                    UserNotificationPref = dto,
                    Provinces = await _userNotificationPrefsService.GetProvincesAsync(),
                    Districts = await _userNotificationPrefsService.GetDistrictsGroupedByProvinceAsync()
                };
                ViewBag.Province = new SelectList(vm.Provinces, "ProvinceId", "Name");
                ViewBag.DistrictsJson = JsonConvert.SerializeObject(vm.Districts);
                return View(vm);
            }

            var userId = GetLoggedInUserId(); 

            var success = await _userNotificationPrefsService.UpdateAsync(dto, userId); 
            if (!success)
                return NotFound();
            TempData["SuccessMessage"] = "Bildirim Tercihi Güncellendi.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();
            var pref = await _userNotificationPrefsService.GetByIdAsync(id);

            if (pref == null || pref.User_id != userId)
                return NotFound();

            return View(pref); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetLoggedInUserId();
            await _userNotificationPrefsService.DeleteAsync(id, userId);

            TempData["SuccessMessage"] = "Bildirim Tercihi Silme Başarılı";
            return RedirectToAction(nameof(Index));
        }


    }
}
