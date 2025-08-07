using Blazorise;
using Kargo_İlan.DTOs.Offer;
using Kargo_İlan.DTOs.Offers;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kargo_İlan.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferServices _offerService;
        private readonly IOfferNotificationService _notificationService;

        public OfferController(IOfferServices offerService, IOfferNotificationService notificationService)
        {
            _offerService = offerService;
            _notificationService = notificationService;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Kullanıcı oturumu geçerli değil.");

            return int.Parse(userIdClaim.Value);
        }


        [HttpGet]
        public async Task<IActionResult> Offers()
        {
            var userId = GetUserIdFromClaims();
            var offerDTOs = await _offerService.GetValidUserOffersAsync(userId);
            return View(offerDTOs);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int freightId)
        {
            var userId = GetUserIdFromClaims();
            var freight = await _offerService.GetFreightByIdAsync(freightId);
            if (freight == null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction("Index", "Freight");
            }

            // Bireysel ilan sahibi kontrolü:
            if (freight.User_id == userId)
            {
                TempData["ErrorMessage"] = "Kendi ilanınıza teklif veremezsiniz.";
                return RedirectToAction("Index", "Freight");
            }

            // EK: Şirket bazlı kontrol
            // İlan sahibinin şirketini al:
            int ownerUserId = freight.User_id;
            int? ownerCompanyId = await _offerService.GetCompanyIdForUserAsync(ownerUserId);
            // Mevcut kullanıcının şirketini al:
            int? currentCompanyId = await _offerService.GetCompanyIdForUserAsync(userId);
            if (ownerCompanyId.HasValue && currentCompanyId.HasValue && ownerCompanyId.Value == currentCompanyId.Value)
            {
                TempData["ErrorMessage"] = "Kendi şirketinizdeki ilana teklif veremezsiniz.";
                return RedirectToAction("Index", "Freight");
            }

            // Mevcut kod:
            if (await _offerService.HasAcceptedOffer(freightId))
            {
                TempData["ErrorMessage"] = "Bu ilana zaten bir teklif kabul edilmiş. Yeni teklif verilemez.";
                return RedirectToAction("Index", "Freight");
            }

            var model = await _offerService.Create(freightId);
            _offerService.SetFreightDetails(ViewData, freight);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create offerDTO)
        {
            var userId = GetUserIdFromClaims();

            // ModelState kontrolü ilk
            if (!ModelState.IsValid)
                return View(offerDTO);

            // FreightId offerDTO.Freight_id varsayılıyor
            int freightId = offerDTO.Freight_id;

            // Freight'i çek
            var freight = await _offerService.GetFreightByIdAsync(freightId);
            if (freight == null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction("Index", "Freight");
            }

            // Bireysel ilan sahibi kontrolü:
            if (freight.User_id == userId)
            {
                TempData["ErrorMessage"] = "Kendi ilanınıza teklif veremezsiniz.";
                return RedirectToAction("Index", "Freight");
            }

            // EK: Şirket bazlı kontrol
            int ownerUserId = freight.User_id;
            int? ownerCompanyId = await _offerService.GetCompanyIdForUserAsync(ownerUserId);
            int? currentCompanyId = await _offerService.GetCompanyIdForUserAsync(userId);
            if (ownerCompanyId.HasValue && currentCompanyId.HasValue && ownerCompanyId.Value == currentCompanyId.Value)
            {
                TempData["ErrorMessage"] = "Kendi şirketinizdeki ilana teklif veremezsiniz.";
                return RedirectToAction("Index", "Freight");
            }

            // Mevcut kod: HasAcceptedOffer kontrolü
            if (await _offerService.HasAcceptedOffer(freightId))
            {
                TempData["ErrorMessage"] = "Bu ilana zaten bir teklif kabul edilmiş. Yeni teklif verilemez.";
                return RedirectToAction("Index", "Freight");
            }

            try
            {
                await _offerService.CreateOfferAsync(offerDTO, userId);
                TempData["Success"] = "Teklif başarıyla oluşturuldu.";
                return RedirectToAction("Offers");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Offers");
            }
        }


        [HttpGet]
        [Route("{controller}/details/{id}")]
        public async Task<IActionResult> Details(int id, [FromQuery] string from)
        {
            var model = await _offerService.GetOfferDetailsAsync(id);
            if (model == null) return NotFound();

            ViewBag.ReturnUrl = from switch
            {
                "Notification" => Url.Action("Index", "Notification"),
                "Offers" => Url.Action("Offers", "Offer"),
                _ => Url.Action("Index", "Home")
            };
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var currentUserId = GetUserIdFromClaims();

            // Servisten DTO al
            var dto = await _offerService.GetOfferUpdateDetailsAsync(id);
            if (dto == null)
                return NotFound();

            // Sahip kontrolü
            if (dto.User_id != currentUserId)
            {
                TempData["Error"] = "Bu teklifi güncelleme yetkiniz yok.";
                return RedirectToAction("Offers");
            }


            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateView dto)
        {
            // 1. Önce OfferId'yi URL'den gelen id ile kesin olarak eşitleyin:
            dto.OfferId = id;

            // 2. User_id override
            var userId = GetUserIdFromClaims();
            dto.User_id = userId;

            // 3. Şimdi ownership kontrolü: Bu aşamada dto.OfferId kesinlikle doğru olmalı (id parametresinden)
            var existing = await _offerService.GetOfferUpdateDetailsAsync(dto.OfferId);
            if (existing == null)
            {
                // Teklif yok veya silinmiş
                TempData["Error"] = "Teklif bulunamadı veya silinmiş.";
                return RedirectToAction("Offers");
            }
            if (existing.User_id != userId)
            {
                // Sahibiniz değilsiniz
                TempData["Error"] = "Bu teklifi güncelleme yetkiniz yok.";
                return RedirectToAction("Offers");
            }

            // 4. ModelState invalid: Price/Description valid değilse read-only alanları yeniden doldur
            if (!ModelState.IsValid)
            {
                // existing zaten doğru OfferId ile dolu
                dto.FreightTitle = existing.FreightTitle;
                dto.AlisYuk = existing.AlisYuk;
                dto.VarisYuk = existing.VarisYuk;
                dto.CargoTur = existing.CargoTur;
                dto.Miktari = existing.Miktari;
                dto.CreateAt = existing.CreateAt;
                dto.Freight_id = existing.Freight_id;
                return View(dto);
            }

            // 5. Güncelleme
            try
            {
                await _offerService.UpdateOfferAsync(dto, new User { User_id = userId });
                TempData["Success"] = "Teklif başarıyla güncellendi.";
                return RedirectToAction("Offers");
            }
            catch (InvalidOperationException ex)
            {
                // Hata fırlatıldıysa read-only alanları yeniden doldur:
                dto.FreightTitle = existing.FreightTitle;
                dto.AlisYuk = existing.AlisYuk;
                dto.VarisYuk = existing.VarisYuk;
                dto.CargoTur = existing.CargoTur;
                dto.Miktari = existing.Miktari;
                dto.CreateAt = existing.CreateAt;
                dto.Freight_id = existing.Freight_id;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }







        // GET: /Offer/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _offerService.GetOfferDetailsAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Teklif bulunamadı.";
                return RedirectToAction("Offers");
            }

            var currentUserId = GetUserIdFromClaims();
            if (model.User_id != currentUserId)
            {
                TempData["Error"] = "Bu teklifi silme yetkiniz yok.";
                return RedirectToAction("Offers");
            }

            return View(model);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserIdFromClaims();
            try
            {
                await _offerService.DeleteOfferAsync(id, userId);
                TempData["Success"] = "Teklif başarıyla silindi.";
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Bu teklifi silme yetkiniz yok.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Offers");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOffer(int Offer_id)
        {
            if (Offer_id == 0)
            {
                TempData["Error"] = "Geçersiz teklif numarası.";
                return RedirectToAction("Offers");
            }

            int userId = GetUserIdFromClaims();
            bool ok = await _offerService.ConfirmOfferAsync(Offer_id, userId);

            if (ok)
                TempData["Success"] = "Teklif başarıyla onaylandı!";
            else
                TempData["Error"] = "Onay işlemi başarısız veya yetkiniz yok.";

            return RedirectToAction("Details", new { id = Offer_id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectOffer(int Offer_id)
        {
            if (Offer_id == 0)
            {
                TempData["Error"] = "Geçersiz teklif numarası.";
                return RedirectToAction("Offers");
            }

            int userId = GetUserIdFromClaims();
            bool ok = await _offerService.RejectOfferAsync(Offer_id, userId);

            if (ok)
                TempData["Success"] = "Teklif reddedildi.";
            else
                TempData["Error"] = "Red işlemi başarısız veya yetkiniz yok.";

            return RedirectToAction("Details", new { id = Offer_id });
        }
    }
}
