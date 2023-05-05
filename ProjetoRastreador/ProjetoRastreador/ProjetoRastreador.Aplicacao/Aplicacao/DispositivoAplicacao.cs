using ProjetoRastreador.Dominio.Entidades;
using ProjetoRastreador.Persistencia.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Aplicacao.Aplicacao
{
    public class DispositivoAplicacao 
    {
        private DispositivoPersistencia dispositivoPersistencia;
        public DispositivoAplicacao()
        {
            dispositivoPersistencia = new DispositivoPersistencia();
        }

        public bool NovoDispositivo(Dispositivo dispositivo)
        {
            // Verifica se o Dispositivo Existe, se nao exixtir Cria
            dispositivo.IdDispositivo = dispositivoPersistencia.VerificarDispositivoExiste(dispositivo.Codigo);
            if (dispositivo.IdDispositivo == Guid.Empty)
            {
                dispositivo.TabelaDados = dispositivoPersistencia.CriarTabelaDados(dispositivo.Codigo);
                if (dispositivo.TabelaDados != string.Empty)
                {
                    dispositivo.VersaoFirmware = 0;
                    dispositivo.IdDispositivo = Guid.NewGuid();
                    if (dispositivoPersistencia.CriarDispositivo(dispositivo) == false)
                    {          
                        return false;
                    }                   
                }
                else
                {
                    return false;
                }
            }

            // linca o Dispositivo ao Usuario
            dispositivo.IdUsuarioDispositivo = dispositivoPersistencia.VerificaDispositivoUsuario(dispositivo.IdUsuario,dispositivo.IdDispositivo);
            if (dispositivo.IdUsuarioDispositivo == Guid.Empty)
            {
                dispositivo.IdUsuarioDispositivo = Guid.NewGuid();
                dispositivoPersistencia.LincarUsuarioDispositivo(dispositivo);
            }
            
            return true;
        }

        public bool ApagarDispositivo(Guid IdUsuarioDispositivo)
        {
            Dispositivo dispositivo = new Dispositivo();
            dispositivo = DadosDispositivo(IdUsuarioDispositivo);

            if (dispositivoPersistencia.ApagaDispositivoUsuario(dispositivo.IdUsuarioDispositivo) == false)
            {
                return false;
            }

            if (dispositivoPersistencia.VerificarDispositivoSendoUsando(dispositivo.IdDispositivo))
            {
                return true;
            }
            else
            {
                if (dispositivoPersistencia.ApagaDispositivo(dispositivo.IdDispositivo))
                {
                    if (dispositivoPersistencia.ApagarTabelaDados(dispositivo.TabelaDados))
                    {
                        return true;
                    }  
                }
                return false;                
            }
        }

        public List<DispositivoStatus> ListaStatusDispositivos(Guid IdUsuario)
        {
            return dispositivoPersistencia.ListaStatusDispositivos(IdUsuario);
        }

        public List<DadosLocalizacaoDispositivo> BuscaDadosLocalizacaoDispositivos(Guid idUsuarioDispositivo, DateTime dataInicio, DateTime dataFim)
        {
            Dispositivo dispositivo = new Dispositivo();

            dispositivo = dispositivoPersistencia.DadosDispositivo(idUsuarioDispositivo);

            return dispositivoPersistencia.BuscaDadosLocalizacaoDispositivos(dispositivo.TabelaDados, dataInicio, dataFim);
        }

        public Dispositivo DadosDispositivo(Guid IdUsuarioDispositivo)
        {
            return dispositivoPersistencia.DadosDispositivo(IdUsuarioDispositivo);
        }

        public bool VerificaDispositivoOnline(Guid IdUsuarioDispositivo)
        {
            Dispositivo dispositivo = new Dispositivo();

            dispositivo = dispositivoPersistencia.DadosDispositivo(IdUsuarioDispositivo);

            if (dispositivoPersistencia.VerificaDispositivoOnline(dispositivo.TabelaDados))
            {
                return true;
            }

            return false;
        }

        public bool SalvaNomeDispositivo(Guid IdUsuarioDispositivo, string Nome)
        {
            return dispositivoPersistencia.SalvaNomeDispositivo(IdUsuarioDispositivo, Nome);
        }

        public bool SalvaEstadoSaidaDispositivo(Guid IdUsuarioDispositivo, bool Estado)
        {
            return dispositivoPersistencia.SalvaEstadoSaidaDispositivo(IdUsuarioDispositivo, Estado);
        }
    
        public Decimal QuantidadeDados(Guid IdUsuarioDispositivo)
        {
            Dispositivo dispositivo = new Dispositivo();

            dispositivo = dispositivoPersistencia.DadosDispositivo(IdUsuarioDispositivo);

            return dispositivoPersistencia.QuantidadeDados(dispositivo.TabelaDados);
        }
       public DadosLocalizacaoDispositivo UltimoDadoRecebido (Guid IdUsuarioDispositivo) 
       { 
            DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
            Dispositivo dispositivo = new Dispositivo();

            dispositivo = dispositivoPersistencia.DadosDispositivo(IdUsuarioDispositivo);
            dadosLocalizacaoDispositivo = dispositivoPersistencia.UltimoDadoRecebido(dispositivo.TabelaDados);

            return dadosLocalizacaoDispositivo;
       }
    }
}
