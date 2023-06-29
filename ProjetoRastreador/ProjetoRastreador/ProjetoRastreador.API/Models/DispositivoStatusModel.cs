namespace ProjetoRastreador.API.Models
{
    public class DispositivoStatusModel
    {       
        public Guid IdUsuarioDispositivo { get; set; }
        public string Nome { get; set; }
        public bool Online { get; set; }
        public string UltimoDadoRecebido { get; set; }

    }
}
