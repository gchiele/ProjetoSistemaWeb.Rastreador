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

        public Guid NovoDispositivo(Dispositivo dispositivo)
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
                    if (dispositivoPersistencia.CriarDispositivo(dispositivo) == Guid.Empty)
                    {          
                        return Guid.Empty;
                    }                   
                }
                else
                {
                    return Guid.Empty;
                }
            }

            // linca o Dispositivo ao Usuario
            dispositivo.IdUsuarioDispositivo = dispositivoPersistencia.VerificaDispositivoUsuario(dispositivo.IdUsuario,dispositivo.IdDispositivo);
            if (dispositivo.IdUsuarioDispositivo == Guid.Empty)
            {
                dispositivo.IdUsuarioDispositivo = Guid.NewGuid();
                dispositivoPersistencia.LincarUsuarioDispositivo(dispositivo);
            }
            
            return dispositivo.IdDispositivo;
        }

        public Guid ApagarDispositivo(Guid IdUsuarioDispositivo)
        {
            Dispositivo dispositivo = new Dispositivo();
            dispositivo = DadosDispositivo(IdUsuarioDispositivo);

            if (dispositivoPersistencia.ApagaDispositivoUsuario(dispositivo.IdUsuarioDispositivo) == Guid.Empty)
            {
                return Guid.Empty;
            }

            Guid IdRetorno;
            IdRetorno = dispositivoPersistencia.VerificarDispositivoSendoUsando(dispositivo.IdDispositivo);
            if (IdRetorno != Guid.Empty)
            {
                return IdRetorno;
            }
            else
            {
                IdRetorno = dispositivoPersistencia.ApagaDispositivo(dispositivo.IdDispositivo);
                if (IdRetorno != Guid.Empty)
                {
                    if (dispositivoPersistencia.ApagarTabelaDados(dispositivo.TabelaDados))
                    {
                        return IdRetorno;
                    }  
                }
                return Guid.Empty;                
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

        public Guid SalvaNomeDispositivo(Guid IdUsuarioDispositivo, string Nome)
        {
            return dispositivoPersistencia.SalvaNomeDispositivo(IdUsuarioDispositivo, Nome);
        }

        public Guid SalvaEstadoSaidaDispositivo(Guid IdUsuarioDispositivo, bool Estado)
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


        public RespostaEntradaDadosDispositivo SalvaDadosLocalizacaoDispositivo (EntradaDadosDispositivo DadosDispositivo)
        {
            RespostaEntradaDadosDispositivo respostaEntradaDadosDispositivo = new RespostaEntradaDadosDispositivo ();
            Dispositivo dispositivo;
            // busca dados dispositivo
            dispositivo = dispositivoPersistencia.DadosDispositivo(DadosDispositivo.CodigoDispositvo);

            if (dispositivo.IdDispositivo != Guid.Empty)
            {
                // Salva os dados de Localizacao
                respostaEntradaDadosDispositivo.Status = "1";

                foreach (DadosLocalizacaoDispositivo dadosLocalizacao in DadosDispositivo.DadosLocalizacao)
                {
                    bool Resultado = dispositivoPersistencia.SalvaLocalizacaoDispositivo(dispositivo.TabelaDados, dadosLocalizacao);

                    if (!Resultado)
                    {
                        respostaEntradaDadosDispositivo.Status = "0";
                    }
                }

                // Atualiza o Firmware atual do dispositivo
                dispositivoPersistencia.UpdateVersaoDispositivo(dispositivo.IdDispositivo, DadosDispositivo.VesaoSoftware);

                // Busca o Estado atual da Saida
                respostaEntradaDadosDispositivo.Saida = "0";
                if (dispositivo.ComandoSaida)
                {
                    respostaEntradaDadosDispositivo.Saida = "1";
                }
            }
            else
            {
                respostaEntradaDadosDispositivo.Status = "1";
                respostaEntradaDadosDispositivo.Saida = "1";
            }
         
            return respostaEntradaDadosDispositivo;
        }
    }
}
