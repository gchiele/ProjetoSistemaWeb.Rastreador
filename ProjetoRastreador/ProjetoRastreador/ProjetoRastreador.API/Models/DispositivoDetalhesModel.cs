namespace ProjetoRastreador.API.Models
{
    public class DispositivoDetalhesModel
    {
        public Guid IdUsuarioDispositivo { get; set; }

        public string Nome { get; set;}
        public bool Online { get; set; }
        public string Codigo { get; set; }
        public string TabelaDados { get; set; }
        public double VersaoFirmware { get; set; }
        public bool ComandoSaida { get; set; }

        public decimal QuantidadeDados { get; set; }

        public string UltimoDadoDataHora { get; set; }
        public double UltimoDadoLatitude { get; set; }
        public double UltimoDadoLongitude { get; set; }
        public double UltimoDadoAltitude { get; set; }
        public decimal UltimoDadoSatelites { get; set; }
        public decimal UltimoDadoSinalOperadora { get; set; }
        public bool UltimoDadoSaida { get; set; }
       
    }
}
