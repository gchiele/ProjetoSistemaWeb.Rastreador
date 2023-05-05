using ProjetoRastreador.Dominio.Entidades;
using ProjetoRastreador.Persistencia.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Aplicacao.Aplicacao
{
    public class UsuarioAplicacao
    {
        private UsuarioPersistencia usuarioPersistencia;
        public UsuarioAplicacao() 
        {
            usuarioPersistencia = new UsuarioPersistencia();
        }

        public Guid UsuarioLogin(Usuario usuario)
        {        
            return usuarioPersistencia.LoginUsuario(usuario);
        }

        public Usuario BuscaUsuario(Guid Id) 
        {
            return usuarioPersistencia.BuscaUsuario(Id);
        }

        public Guid NovoUsuario(Usuario usuario) 
        {
            if (usuarioPersistencia.VerificaExisteEmail(usuario) == Guid.Empty) 
            {
                usuario.Id = Guid.NewGuid();
                return usuarioPersistencia.NovoUsuario(usuario);
            }
            else
            {
                // Usuario ja Existe
                return Guid.Empty;
            }  
        }

        public bool VerificaSenha(Guid IdUsuario, string Senha)
        {
            Usuario usuario = new Usuario();

            usuario = usuarioPersistencia.BuscaUsuario(IdUsuario);

            if (usuario.Senha == Senha)
            {
                return true;
            }

            return false;
        }

        public bool SalvaUsuario(Usuario usuario)
        {
            // Fazer Verificacao de email nao repetido

            return usuarioPersistencia.SalvaUsuario(usuario);
        }

   


    }
}
