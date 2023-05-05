namespace ProjetoRastreador.Web.Models
{
    public class DadosLocalizacaoDispositivoModel
    {
        public string DataHora { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public decimal Satelites { get; set; }
        public decimal SinalOperadora { get; set; }
        public bool Saida { get; set; }
    }
}
