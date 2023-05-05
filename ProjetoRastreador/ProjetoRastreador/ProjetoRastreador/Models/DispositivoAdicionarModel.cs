using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.Web.Models
{
    public class DispositivoAdicionarModel
    {
        [Required(ErrorMessage = "O campo Nome deve ser informado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Codigo deve ser informado")]
        public string Codigo { get; set; }
    }
}
