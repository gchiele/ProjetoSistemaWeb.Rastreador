using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class EntradaDadosDispositivo
    {
        public string CodigoDispositvo { get; set; }
        public double VesaoSoftware { get; set; }
        public List<DadosLocalizacaoDispositivo> DadosLocalizacao { get; set; }
    }
}
