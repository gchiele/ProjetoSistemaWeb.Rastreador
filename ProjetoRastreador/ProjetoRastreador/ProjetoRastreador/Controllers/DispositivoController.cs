using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal.TypeHandlers;
using ProjetoRastreador.Aplicacao.Aplicacao;
using ProjetoRastreador.Dominio.Entidades;
using ProjetoRastreador.Web.Models;

namespace ProjetoRastreador.Web.Controllers
{
    public class DispositivoController : Controller
    {

        // Tela Dispositivos
        public IActionResult Index()
        {
            return RedirectToAction("Dispositivos");
        }
        
        public IActionResult Dispositivos() { 
            return View();
        }

        public IActionResult ListaStatusDispositivos() {
            Guid IdUsuario = Guid.Parse(HttpContext.Session.GetString("IdUsuario"));

            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            var dispositivosStatus = dispositivoAplicacao.ListaStatusDispositivos(IdUsuario);

            // Traduz o objeto do banco para o objeto do Model
            List<DispositivoStatusModel> dispositivosStatusModel = new List<DispositivoStatusModel>();
            for (int i = 0; i < dispositivosStatus.Count; i++)
            {
                dispositivosStatusModel.Add(new DispositivoStatusModel()
                {
                    IdUsuarioDispositivo = dispositivosStatus[i].IdUsuarioDispositivo,
                    Nome = dispositivosStatus[i].Nome,
                    Online = dispositivosStatus[i].Online,
                    UltimoDadoRecebido = dispositivosStatus[i].UltimoDadoRecebido
                });
            }

            return Json(dispositivosStatusModel);
        }



        // Tela adicionar dispositivo
        public IActionResult Adicionar()
        {       
            return View();
        }
        
        // submit adicionar dispositivo
        public IActionResult NovoDispositivo(DispositivoAdicionarModel dispositivoWeb)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            Dispositivo dispositivo = new Dispositivo();

            if (ModelState.IsValid)
            {
                dispositivo.IdUsuario = Guid.Parse(HttpContext.Session.GetString("IdUsuario"));
                dispositivo.Nome = dispositivoWeb.Nome;
                dispositivo.Codigo = dispositivoWeb.Codigo;

                if (dispositivoAplicacao.NovoDispositivo(dispositivo))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Mensagem", "Erro ao Adicionar dispositivo");
                    return View("Adicionar");
                }
            }
            return View("Adicionar");
        }



        // Tela Mapa Real Time
        public IActionResult MapaTempoReal(Guid id)
        {            
            return View(DadosDispositivoMapa(id));
        }

        public IActionResult BuscaDadosMarcador(Guid id)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();

            dadosLocalizacaoDispositivo = dispositivoAplicacao.UltimoDadoRecebido(id);

            DadosLocalizacaoDispositivoModel dadosLocalizacaoDispositivoModel = new DadosLocalizacaoDispositivoModel();

            dadosLocalizacaoDispositivoModel.DataHora = dadosLocalizacaoDispositivo.DataHora;
            dadosLocalizacaoDispositivoModel.Latitude = dadosLocalizacaoDispositivo.Latitude;
            dadosLocalizacaoDispositivoModel.Longitude = dadosLocalizacaoDispositivo.Longitude;
            dadosLocalizacaoDispositivoModel.Satelites = dadosLocalizacaoDispositivo.Satelites;
            dadosLocalizacaoDispositivoModel.Altitude = dadosLocalizacaoDispositivo.Altitude;
            dadosLocalizacaoDispositivoModel.SinalOperadora = dadosLocalizacaoDispositivo.SinalOperadora;
            dadosLocalizacaoDispositivoModel.Saida = dadosLocalizacaoDispositivo.Saida;

            return Json(dadosLocalizacaoDispositivoModel);
        }

        public IActionResult BuscaEstadoBotaoStop(Guid id)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            Dispositivo dispositivo = new Dispositivo();    
            dispositivo = dispositivoAplicacao.DadosDispositivo(id);

            if (dispositivo.ComandoSaida)
            {
                return Json("Bloqueado");
            }
            return Json("Liberado");
        }

        public IActionResult AlteraEstadoBotaoStop(Guid id, string estado)
        {
            bool Estado = false;
            if (estado == "Bloqueado")
            {
                Estado = true;
            }

            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            dispositivoAplicacao.SalvaEstadoSaidaDispositivo(id, Estado);
            return Json(true);
        }



        // Submit Apagar dispositivo
        public IActionResult ExcluirDispositivo(Guid id)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            if (dispositivoAplicacao.ApagarDispositivo(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Mensagem", "Erro ao Excluir Dispositivo");
                return View("Mapa", DadosDispositivoMapa(id));
            }             
        }
        


       
        // Tela Detalhes
        public IActionResult Detalhes(Guid id)
        {
            Dispositivo dispositivo = new Dispositivo();
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
            DispositivoDetalhesModel dispositivoDetalhesModel = new DispositivoDetalhesModel();


            dispositivoDetalhesModel.IdUsuarioDispositivo = id;
            dispositivo = dispositivoAplicacao.DadosDispositivo(id);
            dispositivoDetalhesModel.Nome = dispositivo.Nome;
            dispositivoDetalhesModel.VersaoFirmware = dispositivo.VersaoFirmware;
            dispositivoDetalhesModel.Codigo = dispositivo.Codigo;
            dispositivoDetalhesModel.ComandoSaida = dispositivo.ComandoSaida;
            dispositivoDetalhesModel.TabelaDados = dispositivo.TabelaDados;
            dispositivoDetalhesModel.Online = dispositivoAplicacao.VerificaDispositivoOnline(id);
            dispositivoDetalhesModel.QuantidadeDados = dispositivoAplicacao.QuantidadeDados(id);
            dadosLocalizacaoDispositivo = dispositivoAplicacao.UltimoDadoRecebido(id);
            dispositivoDetalhesModel.UltimoDadoDataHora = dadosLocalizacaoDispositivo.DataHora;
            dispositivoDetalhesModel.UltimoDadoLatitude = dadosLocalizacaoDispositivo.Latitude;
            dispositivoDetalhesModel.UltimoDadoLongitude = dadosLocalizacaoDispositivo.Longitude;
            dispositivoDetalhesModel.UltimoDadoAltitude = dadosLocalizacaoDispositivo.Altitude;
            dispositivoDetalhesModel.UltimoDadoSaida = dadosLocalizacaoDispositivo.Saida;
            dispositivoDetalhesModel.UltimoDadoSatelites = dadosLocalizacaoDispositivo.Satelites;
            dispositivoDetalhesModel.UltimoDadoSinalOperadora = dadosLocalizacaoDispositivo.SinalOperadora;
            
            return View(dispositivoDetalhesModel);
        }



        // Tela Historico
        public IActionResult Historico(Guid id)
        {
            return View(DadosDispositivoMapa(id));
        }

        public IActionResult BuscaDadosMapa(Guid id, DateTime dataInicio, DateTime dataFim)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            List<DadosLocalizacaoDispositivo> dadosLocalizacaoDispositivos = new List<DadosLocalizacaoDispositivo>();           
            List<DadosLocalizacaoDispositivoModel> dadosLocalizacaoDispositivosModel = new List<DadosLocalizacaoDispositivoModel>();

            dadosLocalizacaoDispositivos = dispositivoAplicacao.BuscaDadosLocalizacaoDispositivos(id, dataInicio, dataFim);

            for (int i = 0; i < dadosLocalizacaoDispositivos.Count; i++)
            {
                DadosLocalizacaoDispositivoModel dadosLocalizacaoDispositivoModel = new DadosLocalizacaoDispositivoModel();

                dadosLocalizacaoDispositivoModel.DataHora = dadosLocalizacaoDispositivos[i].DataHora;
                dadosLocalizacaoDispositivoModel.Latitude = dadosLocalizacaoDispositivos[i].Latitude;
                dadosLocalizacaoDispositivoModel.Longitude = dadosLocalizacaoDispositivos[i].Longitude;
                dadosLocalizacaoDispositivoModel.Satelites = dadosLocalizacaoDispositivos[i].Satelites;
                dadosLocalizacaoDispositivoModel.Altitude = dadosLocalizacaoDispositivos[i].Altitude;
                dadosLocalizacaoDispositivoModel.SinalOperadora = dadosLocalizacaoDispositivos[i].SinalOperadora;
                dadosLocalizacaoDispositivoModel.Saida = dadosLocalizacaoDispositivos[i].Saida;

                dadosLocalizacaoDispositivosModel.Add(dadosLocalizacaoDispositivoModel);
            }

            return Json(dadosLocalizacaoDispositivosModel);
        }


        // Tela Editar Dispositivo
        public IActionResult Editar(Guid id)
        {
            return View(DadosDispositivoEditar(id));
        }

        // submit Salvar dispositivo
        public IActionResult SalvarDispositivo(DispositivoEditarModel dispositivoWeb)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
          
            if (ModelState.IsValid)
            {
                if (dispositivoAplicacao.SalvaNomeDispositivo(dispositivoWeb.IdUsuarioDispositivo, dispositivoWeb.Nome))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Mensagem", "Erro ao Editar o Nome do dispositivo");
            }
            return View("Editar", DadosDispositivoEditar(dispositivoWeb.IdUsuarioDispositivo));
        }

        
        private DispositivoEditarModel DadosDispositivoEditar(Guid idUsuarioDispositivo)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            Dispositivo dispositivo = new Dispositivo();
            DispositivoEditarModel dispositivoEditar = new DispositivoEditarModel();

            dispositivo = dispositivoAplicacao.DadosDispositivo(idUsuarioDispositivo);

            dispositivoEditar.Nome = dispositivo.Nome;
            dispositivoEditar.Codigo = dispositivo.Codigo;
            dispositivoEditar.IdUsuarioDispositivo = idUsuarioDispositivo;

            return dispositivoEditar;
        }

        private DispositivoMapaModel DadosDispositivoMapa(Guid idUsuarioDispositivo)
        {
            DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
            Dispositivo dispositivo = new Dispositivo();
            DispositivoMapaModel dispositivoMapa = new DispositivoMapaModel();

            dispositivo = dispositivoAplicacao.DadosDispositivo(idUsuarioDispositivo);

            dispositivoMapa.IdUsuarioDispositivo = dispositivo.IdUsuarioDispositivo;
            dispositivoMapa.Nome = dispositivo.Nome;
            return dispositivoMapa;
        }


    }
}
