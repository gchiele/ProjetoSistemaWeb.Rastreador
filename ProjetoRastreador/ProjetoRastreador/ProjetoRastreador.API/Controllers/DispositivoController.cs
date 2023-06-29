using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoRastreador.API.Models;
using ProjetoRastreador.Aplicacao.Aplicacao;
using ProjetoRastreador.Dominio.Entidades;

namespace ProjetoRastreador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispositivoController : ControllerBase
    {

        // GET: api/Dispositivo/ListaStatusDispositivo/id
        // Busca dados do Usuario pelo id
        [HttpGet("ListaStatusDispositivo/{IdUsuario}")]
        public IActionResult ListaStatusDispositivo(Guid IdUsuario)
        {
            try
            {
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

                return Ok(dispositivosStatusModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST api/Dispositivo/AdicionarDispositivo
        [HttpPost("AdicionarDispositivo")]
        public IActionResult AdicionarDispositivo([FromBody] DispositivoAdicionarModel dispositivoWeb)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                Dispositivo dispositivo = new Dispositivo();

                dispositivo.IdUsuario = dispositivoWeb.IdUsuario;
                dispositivo.Nome = dispositivoWeb.Nome;
                dispositivo.Codigo = dispositivoWeb.Codigo;
                
                return Ok(dispositivoAplicacao.NovoDispositivo(dispositivo));
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/BuscaDadosMarcador/id
        // Busca dados do Marcador
        [HttpGet("BuscaDadosMarcador/{IdUsuarioDispositivo}")]
        public IActionResult BuscaDadosMarcador(Guid IdUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();

                dadosLocalizacaoDispositivo = dispositivoAplicacao.UltimoDadoRecebido(IdUsuarioDispositivo);

                DadosLocalizacaoDispositivoModel dadosLocalizacaoDispositivoModel = new DadosLocalizacaoDispositivoModel();

                dadosLocalizacaoDispositivoModel.DataHora = dadosLocalizacaoDispositivo.DataHora;
                dadosLocalizacaoDispositivoModel.Latitude = dadosLocalizacaoDispositivo.Latitude;
                dadosLocalizacaoDispositivoModel.Longitude = dadosLocalizacaoDispositivo.Longitude;
                dadosLocalizacaoDispositivoModel.Satelites = dadosLocalizacaoDispositivo.Satelites;
                dadosLocalizacaoDispositivoModel.Altitude = dadosLocalizacaoDispositivo.Altitude;
                dadosLocalizacaoDispositivoModel.SinalOperadora = dadosLocalizacaoDispositivo.SinalOperadora;
                dadosLocalizacaoDispositivoModel.Saida = dadosLocalizacaoDispositivo.Saida;

                return Ok(dadosLocalizacaoDispositivoModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/BuscaEstadoBotaoStop/id
        // Busca dados do Botao Stop
        [HttpGet("BuscaEstadoBotaoStop/{IdUsuarioDispositivo}")]
        public IActionResult BuscaEstadoBotaoStop(Guid IdUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                Dispositivo dispositivo = new Dispositivo();
                dispositivo = dispositivoAplicacao.DadosDispositivo(IdUsuarioDispositivo);

                if (dispositivo.ComandoSaida)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/Dispositivo/BloquearVeiculo 
        // Bloqueia o Veiculo
        [HttpPut("BloquearVeiculo")]
        public IActionResult BloquearVeiculo([FromBody] Guid IdUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                return Ok(dispositivoAplicacao.SalvaEstadoSaidaDispositivo(IdUsuarioDispositivo, true));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/Dispositivo/LiberarVeiculo 
        // Bloqueia o Veiculo
        [HttpPut("LiberarVeiculo")]
        public IActionResult LiberarVeiculo([FromBody] Guid IdUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                return Ok(dispositivoAplicacao.SalvaEstadoSaidaDispositivo(IdUsuarioDispositivo, false));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DELETE api/Dispositivo/id
        [HttpDelete("ApagarDispositivo/{IdDispositivo}")]
        public IActionResult ApagarDispositivo(Guid IdDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                if (dispositivoAplicacao.ApagarDispositivo(IdDispositivo) != Guid.Empty)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/DetalhesDispositivo/id
        // Busca dados do Botao Stop
        [HttpGet("DetalhesDispositivo/{IdUsuarioDispositivo}")]
        public IActionResult DetalhesDispositivo(Guid IdUsuarioDispositivo)
        {
            try
            {
                Dispositivo dispositivo = new Dispositivo();
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
                DispositivoDetalhesModel dispositivoDetalhesModel = new DispositivoDetalhesModel();

                dispositivoDetalhesModel.IdUsuarioDispositivo = IdUsuarioDispositivo;
                dispositivo = dispositivoAplicacao.DadosDispositivo(IdUsuarioDispositivo);
                dispositivoDetalhesModel.Nome = dispositivo.Nome;
                dispositivoDetalhesModel.VersaoFirmware = dispositivo.VersaoFirmware;
                dispositivoDetalhesModel.Codigo = dispositivo.Codigo;
                dispositivoDetalhesModel.ComandoSaida = dispositivo.ComandoSaida;
                dispositivoDetalhesModel.TabelaDados = dispositivo.TabelaDados;
                dispositivoDetalhesModel.Online = dispositivoAplicacao.VerificaDispositivoOnline(IdUsuarioDispositivo);
                dispositivoDetalhesModel.QuantidadeDados = dispositivoAplicacao.QuantidadeDados(IdUsuarioDispositivo);
                dadosLocalizacaoDispositivo = dispositivoAplicacao.UltimoDadoRecebido(IdUsuarioDispositivo);
                dispositivoDetalhesModel.UltimoDadoDataHora = dadosLocalizacaoDispositivo.DataHora;
                dispositivoDetalhesModel.UltimoDadoLatitude = dadosLocalizacaoDispositivo.Latitude;
                dispositivoDetalhesModel.UltimoDadoLongitude = dadosLocalizacaoDispositivo.Longitude;
                dispositivoDetalhesModel.UltimoDadoAltitude = dadosLocalizacaoDispositivo.Altitude;
                dispositivoDetalhesModel.UltimoDadoSaida = dadosLocalizacaoDispositivo.Saida;
                dispositivoDetalhesModel.UltimoDadoSatelites = dadosLocalizacaoDispositivo.Satelites;
                dispositivoDetalhesModel.UltimoDadoSinalOperadora = dadosLocalizacaoDispositivo.SinalOperadora;

                return Ok(dispositivoDetalhesModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/BuscaDadosMapa?id=xxxx&dataInicio=xxxx&dataFim=xxxx
        // Busca dados do Mapa
        [HttpGet("BuscaDadosMapa")]     
        public IActionResult BuscaDadosMapa([FromQuery] Guid id, [FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            try
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

                return Ok(dadosLocalizacaoDispositivosModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/Dispositivo/SalvarDispositivo 
        // Salva Ateracoes do dispositivo
        [HttpPut("SalvarDispositivo")]
        public IActionResult SalvarDispositivo([FromBody] DispositivoEditarModel dispositivoWeb)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                return Ok(dispositivoAplicacao.SalvaNomeDispositivo(dispositivoWeb.IdUsuarioDispositivo, dispositivoWeb.Nome));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/DadosDispositivoEditar/id
        // Busca dados do dispositivo para editar
        [HttpGet("DadosDispositivoEditar/{IdUsuarioDispositivo}")]
        public IActionResult DadosDispositivoEditar(Guid idUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                Dispositivo dispositivo = new Dispositivo();
                DispositivoEditarModel dispositivoEditar = new DispositivoEditarModel();

                dispositivo = dispositivoAplicacao.DadosDispositivo(idUsuarioDispositivo);

                dispositivoEditar.Nome = dispositivo.Nome;
                dispositivoEditar.Codigo = dispositivo.Codigo;
                dispositivoEditar.IdUsuarioDispositivo = idUsuarioDispositivo;

                return Ok(dispositivoEditar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Dispositivo/DadosDispositivoMapa/id
        // Busca dados do dispositivo Mapa
        [HttpGet("DadosDispositivoMapa/{IdUsuarioDispositivo}")]
        public IActionResult DadosDispositivoMapa(Guid idUsuarioDispositivo)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                Dispositivo dispositivo = new Dispositivo();
                DispositivoMapaModel dispositivoMapa = new DispositivoMapaModel();

                dispositivo = dispositivoAplicacao.DadosDispositivo(idUsuarioDispositivo);

                dispositivoMapa.IdUsuarioDispositivo = dispositivo.IdUsuarioDispositivo;
                dispositivoMapa.Nome = dispositivo.Nome;

                return Ok(dispositivoMapa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        // POST api/Dispositivo/EntradaDados
        [HttpPost("EntradaDados")]
        public IActionResult EntradaDados([FromBody] DadosESP32Model dadosESP32Model)
        {
            try
            {
                DispositivoAplicacao dispositivoAplicacao = new DispositivoAplicacao();
                EntradaDadosDispositivo entradaDadosDispositivo = new EntradaDadosDispositivo();

                entradaDadosDispositivo.CodigoDispositvo = dadosESP32Model.ChipId;
                entradaDadosDispositivo.VesaoSoftware = dadosESP32Model.Versao;

                // Inicializar a propriedade DadosLocalizacao com uma nova lista
                entradaDadosDispositivo.DadosLocalizacao = new List<DadosLocalizacaoDispositivo>();

                foreach (DadosESP32LocalizacaoModel DadosESP32Localizacao in dadosESP32Model.Dados)
                {
                    DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
                  
                  //  string dataHoraStringOriginal = DadosESP32Localizacao.DataHora;

                    // Converter a string para um objeto DateTime
                  //  DateTime dataHoraOriginal = DateTime.ParseExact(dataHoraStringOriginal, "dd/MM/yyyy HH:mm:ss", null);

                    // Definir o fuso horário original da string (UTC)
                 //   TimeZoneInfo fusoHorarioOriginal = TimeZoneInfo.Utc;

                    // Definir o fuso horário desejado (UTC-3)
                  //  TimeZoneInfo fusoHorarioDesejado = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

                    // Converter a data e hora para o fuso horário desejado
                  //  DateTime dataHoraDesejada = TimeZoneInfo.ConvertTime(dataHoraOriginal, fusoHorarioOriginal, fusoHorarioDesejado);

                    // Converter a data e hora para string no formato desejado
                  //  string dataHoraStringDesejada = dataHoraDesejada.ToString("yyyy-MM-dd HH:mm:ss");


                    dadosLocalizacaoDispositivo.DataHora = DadosESP32Localizacao.DataHora;
                    dadosLocalizacaoDispositivo.Latitude = DadosESP32Localizacao.Lat;
                    dadosLocalizacaoDispositivo.Longitude = DadosESP32Localizacao.Lon;
                    dadosLocalizacaoDispositivo.Altitude = 0;
                    dadosLocalizacaoDispositivo.Saida = (DadosESP32Localizacao.Saida != 0);
                    dadosLocalizacaoDispositivo.SinalOperadora = 0;
                    dadosLocalizacaoDispositivo.Satelites = DadosESP32Localizacao.NumSat;

                    entradaDadosDispositivo.DadosLocalizacao.Add(dadosLocalizacaoDispositivo);
                }
              
                var Resposta = dispositivoAplicacao.SalvaDadosLocalizacaoDispositivo(entradaDadosDispositivo);
             
                return Ok(Resposta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
