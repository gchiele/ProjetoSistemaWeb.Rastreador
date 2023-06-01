using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ProjetoRastreador.Web.Models
{
    public class UsuarioNovoModel
    {
        [Required(ErrorMessage = "O campo Nome deve ser informado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Fone deve ser informado")]
        public string Fone { get; set; }

        [Required(ErrorMessage = "O campo Email deve ser informado")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha deve ser informado")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo Confirma Senha deve ser informado")]
        public string ConfirmaSenha { get; set; }
    }
}
