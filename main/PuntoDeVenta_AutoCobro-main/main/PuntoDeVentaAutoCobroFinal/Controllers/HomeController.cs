using Microsoft.AspNetCore.Mvc;
namespace PuntoDeVentaAutoCobroFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}