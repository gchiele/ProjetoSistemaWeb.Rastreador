using Ftec.ProjetosWeb.Projeto1.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoRastreador.Aplicacao.Aplicacao;
using ProjetoRastreador.Dominio.Entidades;
using ProjetoRastreador.Web.Models;

namespace ProjetoRastreador.Controllers
{
    public class UsuarioController : Controller
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


        public IActionResult Sair()
        {
            HttpContext.Session.Remove("IdUsuario");
            HttpContext.Session.Remove("NomeUsuario");
            return RedirectToAction("Index");
        }


        [HttpPost]

        public IActionResult Login(UsuarioLoginModel usuarioWeb)
        {
            UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
            Usuario usuario = new Usuario();

            usuario.Senha = usuarioWeb.Senha;
            usuario.Email = usuarioWeb.Email;

            if (ModelState.IsValid)
            {
                Guid id = usuarioAplicacao.UsuarioLogin(usuario);
                if (id != Guid.Empty)
                {
                    usuario = usuarioAplicacao.BuscaUsuario(id);
                    HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
                    HttpContext.Session.SetString("NomeUsuario", usuario.Nome);

                    return RedirectToAction("Index", "Dispositivo");
                }
                else
                {
                    ModelState.AddModelError("Mensagem", "Email e/ou Senha Invalido");
                }
            }         
            return View();
        }

        public IActionResult NovoUsuario(UsuarioNovoModel usuarioWeb) 
        {
            UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
            Usuario usuario = new Usuario();

            if (ModelState.IsValid)
            {
                if (usuarioWeb.Senha == usuarioWeb.ConfirmaSenha)
                {
                    usuario.Senha = usuarioWeb.Senha;
                    usuario.Email = usuarioWeb.Email;
                    usuario.Nome = usuarioWeb.Nome;
                    usuario.Fone = usuarioWeb.Fone;

                    if (usuarioAplicacao.NovoUsuario(usuario) != Guid.Empty)
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
                    UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                    UsuarioEditarModel usuarioEditarModel = new UsuarioEditarModel();
                    usuarioEditarModel = DadosUsuarioEditar();

  
                    if(usuarioAplicacao.VerificaSenha(usuarioWeb.Id, usuarioWeb.SenhaAtual))              
                    {                       
                        Usuario usuario = new Usuario();

                        usuario.Id = usuarioWeb.Id;
                        usuario.Senha = usuarioWeb.NovaSenha;
                        usuario.Fone = usuarioWeb.Fone;
                        usuario.Email = usuarioWeb.Email;
                        usuario.Nome = usuarioWeb.Nome;

                        if (usuarioAplicacao.SalvaUsuario(usuario))
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

            UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
            Usuario usuario = new Usuario();
            UsuarioEditarModel usuarioEditarModel = new UsuarioEditarModel();

            usuario = usuarioAplicacao.BuscaUsuario(IdUsuario);

            usuarioEditarModel.Nome = usuario.Nome;
            usuarioEditarModel.Email = usuario.Email;
            usuarioEditarModel.Fone = usuario.Fone;
            usuarioEditarModel.Id = usuario.Id;

            return usuarioEditarModel;
        }


    }
}
