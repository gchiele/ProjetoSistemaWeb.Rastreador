using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.API.Models
{
    public class DispositivoEditarModel
    {
        [Required(ErrorMessage = "O campo Nome deve ser informado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Codigo deve ser informado")]
        public string Codigo { get; set; }

        public Guid IdUsuarioDispositivo { get; set; }
    }
}
