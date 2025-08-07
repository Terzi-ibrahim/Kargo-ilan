using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;
using Kargo_İlan.Data;
using Microsoft.AspNetCore.Authorization;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Services;
using Kargo_İlan.DTOs;

namespace Kargo_İlan.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KargoDbContext _context;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, KargoDbContext context,IHomeService homeService)
    {
        _logger = logger;
        _context = context;
        _homeService = homeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _homeService.GetAllCategoriesAsync();
        ViewBag.Categories = categories;

        var freights = await _homeService.GetLastSixFreightsAsync(); 

        return View(freights);
    }
   
    public IActionResult About()
    {

        return View();
    }
     public IActionResult Contact()
    {

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); 
        }

        var contact = new Contact 
        {
            FullName = model.FullName,
            Email = model.Email,
            Message = model.Message,
            CreatedDate = DateTime.Now
        };

        try
        {
            _context.Contact.Add(contact);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. Teşekkür ederiz!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "İletişim mesajı kaydedilirken bir hata oluştu.");
            TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
        }
        return RedirectToAction("Contact");

    }




}
