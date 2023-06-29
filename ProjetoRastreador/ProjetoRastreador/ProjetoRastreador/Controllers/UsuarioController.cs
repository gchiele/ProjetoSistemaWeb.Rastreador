using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoRastreador.Web.API;
using ProjetoRastreador.Web.Models;
using System.Net.Http;

namespace ProjetoRastreador.Controllers
{
    public class UsuarioController : Controller
    {

        private APIHttpClient httpClient;

        public UsuarioController()
        {
             // httpClient = new APIHttpClient(@"http://localhost:10001/api/");
            httpClient = new APIHttpClient(@"http://24.152.36.26:10001/api/");
        }

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


        public IActionResult Sair()
        {
            HttpContext.Session.Remove("IdUsuario");
            HttpContext.Session.Remove("NomeUsuario");
            return RedirectToAction("Index");
        }

        // Tela Login
        [HttpPost]

        public IActionResult Login(UsuarioLoginModel usuarioWeb)
        {    
            if (ModelState.IsValid)
            {              
                Guid id = httpClient.Put<UsuarioLoginModel>("Usuario/Login", usuarioWeb);

                if (id != Guid.Empty)
                {
                    var usuarioModel = httpClient.Get<UsuarioModel>("usuario/" + id.ToString());
               
                    HttpContext.Session.SetString("IdUsuario", usuarioModel.Id.ToString());
                    HttpContext.Session.SetString("NomeUsuario", usuarioModel.Nome);

                    return RedirectToAction("Index", "Dispositivo");
                }
                else
                {
                    ModelState.AddModelError("Mensagem", "Email e/ou Senha Invalido");
                }
            }         
            return View();
        }


        // Tela Criar Conta
        public IActionResult NovoUsuario(UsuarioNovoModel usuarioWeb) 
        {           
            if (ModelState.IsValid)
            {
                if (usuarioWeb.Senha == usuarioWeb.ConfirmaSenha)
                {
                    var Id = httpClient.Post<UsuarioNovoModel>("Usuario/NovoUsuario", usuarioWeb);

                    if (Id != Guid.Empty)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Mensagem", "Usuario Ja Existe!");
                    }
                }
                else
                {
                    ModelState.AddModelError("Mensagem", "Senhas estao diferentes");  
                }
            }    
            return View("Inscrever");
        }


        // Tela Minha Conta
        public IActionResult MinhaConta()
        {         
            return View(DadosUsuarioEditar());
        }

        public IActionResult SalvarConta(UsuarioEditarModel usuarioWeb)
        {
            if (ModelState.IsValid)
            {
                if (usuarioWeb.NovaSenha == usuarioWeb.ConfirmaNovaSenha)
                {
                    UsuarioVerificarSenha usuarioVerificarSenha = new UsuarioVerificarSenha();
                    usuarioVerificarSenha.Id = usuarioWeb.Id;
                    usuarioVerificarSenha.Senha = usuarioWeb.NovaSenha;

                    Guid Id = httpClient.Put<UsuarioVerificarSenha>("Usuario/VerificaSenha", usuarioVerificarSenha);

                    if(Id != Guid.Empty)              
                    {
                        Id = httpClient.Put<UsuarioEditarModel>("Usuario/SalvarConta", usuarioWeb);
                        if (Id != Guid.Empty)
                        {
                            return RedirectToAction("Index", "Dispositivo");
                        }
                        ModelState.AddModelError("Mensagem", "Erro ao Salvar conta! ");
                    }
                    ModelState.AddModelError("Mensagem", "'Senha Atual' não corresponde a senha da sua conta! ");
                }
                ModelState.AddModelError("Mensagem", "Campos 'Nova Senha' e 'Confirma Nova Senha' não sao Iguais! ");
            }
            return View("MinhaConta", DadosUsuarioEditar());
        }

        private UsuarioEditarModel DadosUsuarioEditar()
        {
            Guid IdUsuario = Guid.Parse(HttpContext.Session.GetString("IdUsuario"));

            var usuarioEditarModel = httpClient.Get<UsuarioEditarModel>("usuario/" + IdUsuario.ToString());

            return usuarioEditarModel;
        }


    }
}
