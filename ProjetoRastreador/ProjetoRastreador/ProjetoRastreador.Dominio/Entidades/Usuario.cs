using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Dominio.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Fone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario()
        {
            Id = Guid.Empty;
            Nome = string.Empty;
            Fone = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
        }
    }
}
