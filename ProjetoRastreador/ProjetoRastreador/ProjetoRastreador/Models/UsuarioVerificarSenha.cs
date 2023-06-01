using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.Web.Models
{
    public class UsuarioVerificarSenha
    {
        public Guid Id { get; set; }

        public string Senha { get; set; }
    }
}
