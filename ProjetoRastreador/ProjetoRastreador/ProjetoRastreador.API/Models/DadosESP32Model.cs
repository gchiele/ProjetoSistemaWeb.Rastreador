using System.ComponentModel.DataAnnotations;

namespace ProjetoRastreador.API.Models
{
    public class DadosESP32Model
    {
        public string ChipId { get; set; }
        public double Versao { get; set; }
        public List<DadosESP32LocalizacaoModel> Dados { get; set; }
    }
}
