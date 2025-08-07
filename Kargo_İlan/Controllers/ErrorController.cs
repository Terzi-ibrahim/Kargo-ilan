using Microsoft.AspNetCore.Mvc;

namespace Kargo_İlan.Controllers
{
    public class ErrorController : Controller
    {
      
        [Route("Error/{code?}")]
        public IActionResult HandleError(int? code)
        {
            int errorCode = code ?? 404;

            ViewData["ErrorCode"] = errorCode;

            ViewData["ErrorMessage"] = errorCode switch
            {
                404 => "Aradığınız sayfa taşınmış, kaldırılmış veya hiç var olmamış olabilir.",
                405 => "İzin verilmeyen yöntem kullanıldı.",
                500 => "Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                _ => "Beklenmeyen bir hata oluştu."
            };

            return View("NotFound");
        }
    


    }
}
