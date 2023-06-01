using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoRastreador.Aplicacao.Aplicacao;
using ProjetoRastreador.Dominio.Entidades;
using ProjetoRastreador.API.Models;
using System.Collections.Specialized;

namespace ProjetoRastreador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/<UsuarioController>/id
        // Busca dados usuario pelo id
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                Usuario usuario = new Usuario();

                usuario = usuarioAplicacao.BuscaUsuario(id);
               
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }    


        // PUT api/Usuario/Login   <UsuarioLoginModel>
        // Verifica Login
        [HttpPut("Login")]
        public IActionResult Login([FromBody] UsuarioLoginModel usuarioWeb)
        {
            try
            {
                UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                Usuario usuario = new Usuario();

                usuario.Senha = usuarioWeb.Senha;
                usuario.Email = usuarioWeb.Email;

                Guid id = usuarioAplicacao.UsuarioLogin(usuario);      
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/Usuario/VerificaSenha/   <UsuarioEditarModel>
        // VerificaSenha
        [HttpPut("VerificaSenha")]
        public IActionResult VerificaSenha([FromBody] UsuarioVerificarSenha usuarioWeb)
        {
            try
            {
                UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                if(usuarioAplicacao.VerificaSenha(usuarioWeb.Id, usuarioWeb.Senha))
                {
                    return Ok(usuarioWeb.Id);
                }
                return Ok(Guid.Empty);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<UsuarioLoginModel>  UsuarioEditarModel
        // Salva Conta do Usuario
        [HttpPut("SalvarConta")]    
        public IActionResult SalvarConta([FromBody] UsuarioEditarModel usuarioWeb)
        {
            try
            {
                UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                Usuario usuario = new Usuario();

                usuario.Id = usuarioWeb.Id;
                usuario.Senha = usuarioWeb.NovaSenha;
                usuario.Fone = usuarioWeb.Fone;
                usuario.Email = usuarioWeb.Email;
                usuario.Nome = usuarioWeb.Nome;

                if (usuarioAplicacao.SalvaUsuario(usuario))
                {
                    return Ok(usuarioWeb.Id);
                }
                return Ok(Guid.Empty);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        // POST api/<UsuarioController>
        [HttpPost("NovoUsuario")]
        public IActionResult NovoUser([FromBody] UsuarioNovoModel usuarioWeb)
        {
            try
            {
                UsuarioAplicacao usuarioAplicacao = new UsuarioAplicacao();
                Usuario usuario = new Usuario();

                usuario.Senha = usuarioWeb.Senha;
                usuario.Email = usuarioWeb.Email;
                usuario.Nome = usuarioWeb.Nome;
                usuario.Fone = usuarioWeb.Fone;

                Guid Id = usuarioAplicacao.NovoUsuario(usuario);

                return Ok(Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
