using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.Web.Models
{
    public class UsuarioEditarModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Nome deve ser informado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Fone deve ser informado")]
        public string Fone { get; set; }

        [Required(ErrorMessage = "O campo Email deve ser informado")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha Atual deve ser informado")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "O campo Nova Senha deve ser informado")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "O campo Confirma Nova Senha deve ser informado")]
        public string ConfirmaNovaSenha { get; set; }
    }
}
