using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class RespostaEntradaDadosDispositivo
    {
        public string Status { get; set; }
        public string Saida { get; set; }   
      
        public RespostaEntradaDadosDispositivo()
        {
            Status = string.Empty;
            Saida = string.Empty;
        }
    }
}
