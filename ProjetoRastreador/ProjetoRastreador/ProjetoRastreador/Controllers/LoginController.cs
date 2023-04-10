using Microsoft.AspNetCore.Mvc;

namespace ProjetoRastreador.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult EsqueceuSenha() 
        {
            return View();
        }

        public IActionResult Inscrever()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }
    }
}
