using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class Dispositivo
    {
        public Guid IdDispositivo { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdUsuarioDispositivo { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string TabelaDados { get; set; }
        public double VersaoFirmware { get; set; }
        public bool ComandoSaida { get; set; }


        public Dispositivo()
        {
            IdDispositivo = Guid.Empty;
            IdUsuario = Guid.Empty;
            IdUsuarioDispositivo = Guid.Empty;
            Nome = string.Empty;
            Codigo = string.Empty;
            TabelaDados = string.Empty;
            VersaoFirmware = 0.3;
            ComandoSaida = false;   
        }


    }
}
