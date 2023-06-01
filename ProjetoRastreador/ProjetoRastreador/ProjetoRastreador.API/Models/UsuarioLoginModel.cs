using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.API.Models
{
    public class UsuarioLoginModel
    {
        [Required(ErrorMessage = "O campo Email deve ser informado")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha deve ser informado")]
        public string Senha { get; set; }
    }
}
