using Kargo_İlan.DTOs.Freight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Kargo_İlan.Interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Kargo_İlan.Controllers
{
   
    public class FreightController : Controller
    {
        private readonly IFreightService _freightService;
        private readonly INotificationService _notificationService;
     
        public FreightController(IFreightService freight, INotificationService notificationService)
        {
            _freightService = freight;
            _notificationService = notificationService;
        }
        private int GetUserId()
        {
            return Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
   
        [HttpGet]
        public async Task<IActionResult> Index(
            string searchText,
            int? yukilId,
            int? yukilceId,
            int? varisilId,
            int? varisilceId,
            int? vehicleId,
            int? categoryId,
            int? cargoid,
            int page = 1)
        {
            int pageSize = 20;

            // Filtreye göre veri çek
            List<FreightIndexDto> allFreights;
            if (!string.IsNullOrWhiteSpace(searchText) || yukilId.HasValue || yukilceId.HasValue || varisilId.HasValue || varisilceId.HasValue || vehicleId.HasValue || categoryId.HasValue || cargoid.HasValue)
            {
                allFreights = await _freightService.GetFilteredFreightsAsync(searchText, yukilId, yukilceId, varisilId, varisilceId, vehicleId, categoryId, cargoid);
            }
            else
            {
                allFreights = await _freightService.GetAllFreightsAsync();
            }

            int totalItems = allFreights.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedFreights = allFreights.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Filtre seçeneklerini çek
            var filterOptions = await _freightService.GetFilterOptionsAsync();
        

            // Kullanıcı teklif bilgileri
            var userId = GetUserId();
            var (userOffers, acceptedOffers) = await _freightService.GetUserOfferStatusAsync(userId);

            ViewBag.UserId = userId;
            ViewBag.UserOffers = userOffers;
            ViewBag.AcceptedOffers = acceptedOffers;

            // ViewBag'e filtre verileri
            ViewBag.Provinces = filterOptions.Provinces;
            ViewBag.Vehicles = filterOptions.Vehicles;
            ViewBag.Categories = filterOptions.Categories;
            ViewBag.CargoType = filterOptions.CargoType;

            // İlçe bilgilerini JSON olarak geçiyoruz (ProvinceId → List of Districts)
            ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                filterOptions.Districts.ToDictionary(
                    d => d.Key,
                    d => d.Value.Select(v => new { Id = v.Id, Name = v.Name }).ToList()
                )
            );

            // Seçili filtreler
            ViewBag.SelectedSearchText = searchText;
            ViewBag.SelectedIlId = yukilId;
            ViewBag.SelectedIlceId = yukilceId;
            ViewBag.SelectedVarisIlId = varisilId;
            ViewBag.SelectedVarisIlceId = varisilceId;
            ViewBag.SelectedVehicleId = vehicleId;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedCargoid = cargoid;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedFreights);
        }




        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FreightCreate(string returnUrl = null)
        {
            try
            {
                var model = await _freightService.GetFreightCreateViewModelAsync();
                ViewBag.CargoTypes = model.CargoTypes;
                ViewBag.CategoryTypes = model.CategoryTypes;
                ViewBag.VehicleTypes = model.VehicleTypes;
                ViewBag.Province = model.Provinces
                    .Select(p => new SelectListItem
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
                ViewBag.ReturnUrl = returnUrl switch
                {
                    "Notification" => Url.Action("Index", "Notification"),
                    "MyFreight" => Url.Action("MyFreight", "Freight"),
                    "Freight" => Url.Action("Index", "Freight"),
                    _ => Url.Action("Index", "Home")
                };

                return View(model.Freight);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Veriler yüklenirken bir hata oluştu: " + ex.Message);
                return View(new FreightCreate());
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FreightCreate(FreightCreate model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                // ViewBag'leri yeniden doldur
                var viewModel = await _freightService.GetFreightCreateViewModelAsync();

                ViewBag.CargoTypes = viewModel.CargoTypes;
                ViewBag.CategoryTypes = viewModel.CategoryTypes;
                ViewBag.VehicleTypes = viewModel.VehicleTypes;
                ViewBag.Province = viewModel.Provinces
                    .Select(p => new SelectListItem
                    {
                        Value = p.Province_id.ToString(),
                        Text = p.ProvinceName
                    }).ToList();
                ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                    viewModel.Districts.ToDictionary(
                        g => g.Key.ToString(),
                        g => g.Value.Select(d => new { Id = d.Id, Name = d.Name })
                    )
                );

                ViewBag.ReturnUrl = returnUrl switch
                {
                    "Notification" => Url.Action("Index", "Notification"),
                    "MyFreight" => Url.Action("MyFreight", "Freight"),
                    "Freight" => Url.Action("Index", "Freight"),
                    _ => Url.Action("Index", "Home")
                };

                // Kullanıcıya hataları göstermek için sayfayı tekrar dönüyoruz
                return View(model);
            }

            int userId = GetUserId();
            await _freightService.CreateFreightAsync(model, userId);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyFreight()
        {
            int user = GetUserId();
            var userListing = await _freightService.MyFreight(user);
            ViewBag.CargoUnitMap = new Dictionary<int, string>
            {

                { 10, "kg" },
                { 20, "kg" },
                { 30, "adet" },
                { 40, "litre" },
                { 50, "litre" },
                { 60, "litre" },
                { 70, "adet" },
                { 80, "" }
            };

       
            return View(userListing);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id, string returnUrl = null)
        {
            var freightDto = await _freightService.GetFreightDetailAsync(id);
            if (freightDto == null)
                return NotFound();
            ViewBag.CargoUnitMap = new Dictionary<int, string>
            {
                { 10, "kg" },
                { 20, "kg" },
                { 30, "adet" },
                { 40, "litre" },
                { 50, "litre" },
                { 60, "litre" },
                { 70, "adet" },
                { 80, "" }
            };

            ViewBag.ReturnUrl = returnUrl switch
            {
                "FreightIndex" => Url.Action("Index", "Freight"),
                "MyFreight" => Url.Action("MyFreight", "Freight"),
                "Home" => Url.Action("Index", "Home"),
                _ => Url.Action("Index", "Freight")
            };

            return View(freightDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FreightEdit(int id)
        {
            try
            {               
                var model = await _freightService.GetFreightEditViewModelAsync();

           
                var freight = await _freightService.GetFreightByIdAsync(id);
            
                if (freight == null)
                {
                    return NotFound();
                }
                var editModel = new FreightCreate
                {
                    Title = freight.Title,
                    Description = freight.Description,
                    Miktar = freight.Miktar,
                    Category_id = freight.Category_id,
                    Cargo_id = freight.Cargo_id,
                    VehicleType_id = freight.VehicleType_id,
                    YuklemeIlId = freight.Yuklemeil_id,
                    YuklemeIlceId = freight.Yuklemeilce_id, 
                    VarisIlId = freight.Varisil_id,
                    VarisIlceId = freight.Varisilce_id 
                };


                ViewBag.CargoTypes = model.CargoTypes;
                ViewBag.CategoryTypes = model.CategoryTypes;
                ViewBag.VehicleTypes = model.VehicleTypes;
                ViewBag.Province = model.Provinces
                    .Select(p => new SelectListItem
                    {
                        Value = p.Province_id.ToString(),
                        Text = p.ProvinceName
                    }).ToList();


                // Districts verilerini Province_id'ye göre gruplayarak JSON formatına çeviriyoruz
                ViewBag.DistrictsJson = JsonConvert.SerializeObject(
                    model.Districts.ToDictionary(
                        // Her bir KeyValuePair'ın Key'i Province_id'yi temsil eder
                        g => g.Key.ToString(),
                        g => g.Value.Select(d => new { Id = d.Id, Name = d.Name }) // DistrictDto'dan Id ve Name alıyoruz
                    )
                );

                // EditModel'i view'e gönder
                return View(editModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Veriler yüklenirken bir hata oluştu: " + ex.Message);
                return View(new FreightCreate());
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FreightEdit(int id, FreightCreate model)
        {
            FreightCreateViewDto viewModel = null;

            if (!ModelState.IsValid)
            {
                viewModel = await _freightService.GetFreightCreateViewModelAsync();

                ViewBag.CargoType = viewModel.CargoTypes;
                ViewBag.CategoryType = viewModel.CategoryTypes;
                ViewBag.VehicleType = viewModel.VehicleTypes;
                ViewBag.Province = viewModel.Provinces;
                ViewBag.Districts = viewModel.Districts;

                return View(model);
            }

            var userId = GetUserId();

            var result = await _freightService.EditFreightAsync(id, model, userId);

            if (result)
            {
                return RedirectToAction("MyFreight");
            }

            // Güncelleme başarısızsa yeniden ViewBag verilerini doldur
            viewModel = await _freightService.GetFreightCreateViewModelAsync();

            ViewBag.CargoType = viewModel.CargoTypes;
            ViewBag.CategoryType = viewModel.CategoryTypes;
            ViewBag.VehicleType = viewModel.VehicleTypes;
            ViewBag.Province = viewModel.Provinces;
            ViewBag.Districts = viewModel.Districts;

            ModelState.AddModelError("", "İlan güncellenirken bir hata oluştu.");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FreightDelete(int id)
        {
            var userId = GetUserId();
          
            var listing = await _freightService.GetFreightForDeleteAsync(id,userId);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> FreightDeleteConfirmed(int id)
        {
            var userId = GetUserId(); 

            var result = await _freightService.SoftDeleteFreightAsync(id, userId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("MyFreight");
        }
        
    }
}
