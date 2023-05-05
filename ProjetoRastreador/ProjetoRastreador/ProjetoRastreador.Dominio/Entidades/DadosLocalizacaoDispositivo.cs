using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class DadosLocalizacaoDispositivo
    {
        public string DataHora { get; set; }
        public double Latitude { get; set; }   
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public decimal Satelites { get; set; }
        public decimal SinalOperadora { get; set; }
        public bool Saida {get; set;}

        public DadosLocalizacaoDispositivo()
        {
            DataHora = string.Empty;
            Latitude = 0;
            Longitude = 0;
            Altitude = 0;
            Satelites = 0;
            SinalOperadora = 0;
            Saida = false;
        }
    }
}
