using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.API.Models
{
    public class DadosESP32LocalizacaoModel
    {
        public string DataHora { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public decimal NumSat { get; set; }
        public decimal Saida { get; set; }
    }
}
