using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.API.Models
{
    public class UsuarioVerificarSenha
    {
        public Guid Id { get; set; }

        public string Senha { get; set; }
    }
}
