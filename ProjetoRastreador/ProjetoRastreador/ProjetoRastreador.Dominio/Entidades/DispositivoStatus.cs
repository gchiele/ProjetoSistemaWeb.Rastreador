using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class DispositivoStatus : Dispositivo
    {
        public bool Online { get; set; }
        public string UltimoDadoRecebido { get; set; }

        public DispositivoStatus()
        {
            Online = false;
            UltimoDadoRecebido = string.Empty;
        }
    }
}
