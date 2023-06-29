using Microsoft.AspNetCore.Mvc;
using ProjetoRastreador.Web.API;
using ProjetoRastreador.Web.Models;
using System.Globalization;

namespace ProjetoRastreador.Web.Controllers
{
    public class DispositivoController : Controller
    {
        private APIHttpClient httpClient;

        public DispositivoController()
        {
             // httpClient = new APIHttpClient(@"http://localhost:10001/api/");
            httpClient = new APIHttpClient(@"http://24.152.36.26:10001/api/");
        }

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

            var dispositivoStatusModel = httpClient.Get<List<DispositivoStatusModel>>("dispositivo/ListaStatusDispositivo/" + IdUsuario.ToString());

            return Json(dispositivoStatusModel);    
        }



        // Tela adicionar dispositivo
        public IActionResult Adicionar()
        {       
            return View();
        }
        
        // submit adicionar dispositivo
        public IActionResult NovoDispositivo(DispositivoAdicionarModel dispositivoWeb)
        {       
            if (ModelState.IsValid)
            {
                dispositivoWeb.IdUsuario = Guid.Parse(HttpContext.Session.GetString("IdUsuario"));

                var Id = httpClient.Post<DispositivoAdicionarModel>("Dispositivo/AdicionarDispositivo", dispositivoWeb);
              
                if (Id != Guid.Empty)
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
            var dadosLocalizacaoDispositivoModel = httpClient.Get<DadosLocalizacaoDispositivoModel>("dispositivo/BuscaDadosMarcador/" + id.ToString());

            return Json(dadosLocalizacaoDispositivoModel);
        }

        public IActionResult BuscaEstadoBotaoStop(Guid id)
        {
            var comandoSaida = httpClient.Get<bool>("dispositivo/BuscaEstadoBotaoStop/" + id.ToString());

            if (comandoSaida)
            {
                return Json("Bloqueado");
            }
            return Json("Liberado");
        }

        public IActionResult AlteraEstadoBotaoStop(Guid id, string estado)
        {
          
            if (estado == "Bloqueado")
            {
                return Json(httpClient.Put<Guid>("dispositivo/BloquearVeiculo/", id));
            }
            else
            {
                return Json(httpClient.Put<Guid>("dispositivo/LiberarVeiculo/", id));
            }
        }



        // Submit Apagar dispositivo
        public IActionResult ExcluirDispositivo(Guid id)
        {   
            if (httpClient.Delete<bool>("dispositivo/ApagarDispositivo/", id))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Mensagem", "Erro ao Excluir Dispositivo");
            return View("Mapa", DadosDispositivoMapa(id));          
        }
        


       
        // Tela Detalhes
        public IActionResult Detalhes(Guid id)
        {
            var dispositivoDetalhesModel = httpClient.Get<DispositivoDetalhesModel>("dispositivo/DetalhesDispositivo/" + id.ToString());
                
            return View(dispositivoDetalhesModel);
        }



        // Tela Historico
        public IActionResult Historico(Guid id)
        {
            return View(DadosDispositivoMapa(id));
        }

        public IActionResult BuscaDadosMapa(Guid id, DateTime dataInicio, DateTime dataFim)
        {

            string requisicao = "dispositivo/BuscaDadosMapa?id=" + id.ToString() + "&dataInicio=" + dataInicio.ToString("yyyy-MM-ddTHH:mm:ss") + "&dataFim=" + dataFim.ToString("yyyy-MM-ddTHH:mm:ss");
            var dadosLocalizacaoDispositivoModel = httpClient.Get<List<DadosLocalizacaoDispositivoModel>>(requisicao);

            return Json(dadosLocalizacaoDispositivoModel);
        }


        // Tela Editar Dispositivo
        public IActionResult Editar(Guid id)
        {
            return View(DadosDispositivoEditar(id));
        }

        // submit Salvar dispositivo
        public IActionResult SalvarDispositivo(DispositivoEditarModel dispositivoWeb)
        {
                     
            if (ModelState.IsValid)
            {
                if (httpClient.Put<DispositivoEditarModel>("dispositivo/SalvarDispositivo/", dispositivoWeb) != Guid.Empty)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Mensagem", "Erro ao Editar o Nome do dispositivo");
            }
            return View("Editar", DadosDispositivoEditar(dispositivoWeb.IdUsuarioDispositivo));
        }

        
        private DispositivoEditarModel DadosDispositivoEditar(Guid idUsuarioDispositivo)
        {
            var dispositivoEditarModel = httpClient.Get<DispositivoEditarModel>("dispositivo/DadosDispositivoEditar/" + idUsuarioDispositivo.ToString());

            return dispositivoEditarModel;
        }

        private DispositivoMapaModel DadosDispositivoMapa(Guid idUsuarioDispositivo)
        {
            var dispositivoMapaModel = httpClient.Get<DispositivoMapaModel>("dispositivo/DadosDispositivoMapa/" + idUsuarioDispositivo.ToString());

            return dispositivoMapaModel;
        }


    }
}
