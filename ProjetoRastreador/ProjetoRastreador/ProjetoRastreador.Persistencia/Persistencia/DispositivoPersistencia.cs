using Npgsql;
using NpgsqlTypes;
using ProjetoRastreador.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Persistencia.Persistencia
{
    public class DispositivoPersistencia
    {
        private string connectionString = "Server = 24.152.36.26; Port = 10000; Database = rastreador; User Id = postgres; Password = (!-!1&L&;";
        //private string connectionString = "Server = localhost; Port = 5432; Database = rastreador; User Id = postgres; Password = (!-!1&L&;";



        public DispositivoPersistencia() {}


        // Criar dispositivo
        public Guid CriarDispositivo(Dispositivo dispositivo)
        {         
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "INSERT INTO public.dispositivos (id, codigo, tabela_dados, versao_firmware, comando_saida) VALUES(@id, @codigo, @tabela_dados, @versao_firmware, @comando_saida)";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                       
                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@id", dispositivo.IdDispositivo);
                    comando.Parameters.AddWithValue("@codigo", dispositivo.Codigo);
                    comando.Parameters.AddWithValue("@tabela_dados", dispositivo.TabelaDados);
                    comando.Parameters.AddWithValue("@versao_firmware", dispositivo.VersaoFirmware);
                    comando.Parameters.AddWithValue("@comando_saida", dispositivo.ComandoSaida);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();
                }
            }
            return dispositivo.IdDispositivo;
        }

        public string CriarTabelaDados(string CodigoDispositivo)
        {
            string NomeTabela = "dados_dispositivo_" + CodigoDispositivo.ToLower(); ;
            //string NomeTabela = "dados_dispositivo_123abc";

            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "CREATE TABLE public."+ NomeTabela + " (data_hora timestamp NOT NULL,latitude float8 NULL,longitude float8 NULL,altitude float4 NULL,satelites numeric NULL,sinal_operadora numeric NULL,saida bool NULL);";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                  
                    // adiciona os parâmetros à consulta SQL
                    //comando.Parameters.AddWithValue("@Tabela", NomeTabela);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();
                }
            }
            return NomeTabela;
        }

        public Guid VerificarDispositivoExiste(string CodigoDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT id FROM dispositivos WHERE codigo = @codigo;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@codigo", CodigoDispositivo);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetGuid(0);
                        }
                        else
                        {
                            return Guid.Empty;
                        }
                    }
                }
            }
        }

        public Guid VerificaDispositivoUsuario(Guid IdUsuario, Guid IdDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT id FROM usuarios_dispositivos WHERE id_usuario = @id_usuario AND id_dispositivo = @id_dispositivo;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id_usuario", IdUsuario);
                    comando.Parameters.AddWithValue("@id_dispositivo", IdDispositivo);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetGuid(0);
                        }
                        else
                        {
                            return Guid.Empty;
                        }
                    }
                }
            }
        }

        public Guid LincarUsuarioDispositivo(Dispositivo dispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "INSERT INTO public.usuarios_dispositivos (id, id_usuario, id_dispositivo, nome) VALUES(@id, @id_usuario, @id_dispositivo, @nome)";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@id", dispositivo.IdUsuarioDispositivo);
                    comando.Parameters.AddWithValue("@id_usuario", dispositivo.IdUsuario);
                    comando.Parameters.AddWithValue("@id_dispositivo", dispositivo.IdDispositivo);
                    comando.Parameters.AddWithValue("@nome", dispositivo.Nome);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();
                }
            }
            return dispositivo.IdUsuarioDispositivo;
        }     
        

        // Apagar dispositivo
        public Guid VerificarDispositivoSendoUsando(Guid IdDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT id FROM public.usuarios_dispositivos WHERE id_dispositivo = @id_dispositivo;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id_dispositivo", IdDispositivo);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return IdDispositivo;
                        }
                        else
                        {
                            return Guid.Empty;
                        }
                    }
                }
            }
        }
        
        public bool ApagarTabelaDados(string NomeTabelaDados)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "DROP TABLE "+ NomeTabelaDados;

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    int rowsAffected = comando.ExecuteNonQuery();

                    return rowsAffected < 0;
                }
            }
        }

        public Guid ApagaDispositivo(Guid IdDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "DELETE FROM public.dispositivos WHERE id = @id;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", IdDispositivo);

                    int rowsAffected = comando.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return IdDispositivo;
                    }
                    return Guid.Empty;
                }
            }
        }
        
        public Guid ApagaDispositivoUsuario(Guid IdUsuarioDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "DELETE FROM public.usuarios_dispositivos WHERE id = @id;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", IdUsuarioDispositivo);

                    int rowsAffected = comando.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return IdUsuarioDispositivo;
                    }
                    return Guid.Empty;
                }
            }
        }


        // Salvar dispositivo
        public Guid SalvaNomeDispositivo(Guid IdUsuarioDispositivo, string Nome)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "UPDATE public.usuarios_dispositivos SET nome = @nome WHERE id = @id ;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", Nome);
                    comando.Parameters.AddWithValue("@id", IdUsuarioDispositivo);

                    int rowsAffected = comando.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return IdUsuarioDispositivo;
                    }
                    return Guid.Empty;
                }
            }
        }

        public Guid SalvaEstadoSaidaDispositivo(Guid IdUsuarioDispositivo, bool Estado)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "UPDATE public.dispositivos d SET comando_saida = @estado FROM public.usuarios_dispositivos ud WHERE d.id = ud.id_dispositivo AND ud.id = @id;;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@estado", Estado);
                    comando.Parameters.AddWithValue("@id", IdUsuarioDispositivo);

                    int rowsAffected = comando.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        return IdUsuarioDispositivo;
                    }
                    return Guid.Empty;
                }
            }
        }




        // Busca dados Dispositivo
        public DadosLocalizacaoDispositivo UltimoDadoRecebido(string TabelaDados)
        {
            using (var con = new Npgsql.NpgsqlConnection(connectionString))
            {
                con.Open();
                using (var comando = new NpgsqlCommand())
                {
                    comando.Connection = con;
                    comando.CommandText = "SELECT data_hora, latitude, longitude, altitude, satelites, sinal_operadora, saida FROM "+ TabelaDados +" ORDER BY data_hora DESC LIMIT 1;";

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
                        if (reader.Read())
                        {
                            dadosLocalizacaoDispositivo.DataHora = reader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss");
                            dadosLocalizacaoDispositivo.Latitude = reader.GetDouble(1);
                            dadosLocalizacaoDispositivo.Longitude = reader.GetDouble(2);
                            dadosLocalizacaoDispositivo.Altitude = reader.GetDouble(3);
                            dadosLocalizacaoDispositivo.Satelites = reader.GetDecimal(4);
                            dadosLocalizacaoDispositivo.SinalOperadora = reader.GetDecimal(5);
                            dadosLocalizacaoDispositivo.Saida = reader.GetBoolean(6);
                        }
                        
                        return dadosLocalizacaoDispositivo;
                    }

                }
            }
        }

        public bool VerificaDispositivoOnline(string TabelaDados, int MinutosDecremento = 3) 
        {
            using (var con = new Npgsql.NpgsqlConnection(connectionString))
            {
                con.Open();
                using (var comando = new NpgsqlCommand())
                {
                    DateTime dataHoraAtual = DateTime.Now;
                    DateTime dataHoraCalculada = dataHoraAtual.AddMinutes(-MinutosDecremento);

                    comando.Connection = con;
                    comando.CommandText = "SELECT data_hora FROM "+ TabelaDados + " WHERE data_hora >= @dataHora ORDER BY data_hora DESC LIMIT 1;";
                    comando.Parameters.AddWithValue("@dataHora", dataHoraCalculada);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public List<DispositivoStatus> ListaStatusDispositivos(Guid IdUsuario)
        {
            using (var con = new Npgsql.NpgsqlConnection(connectionString))
            {
                con.Open();
                using (var comando = new NpgsqlCommand())
                {
                    comando.Connection = con;
                    comando.CommandText = "SELECT d.codigo, d.tabela_dados, d.versao_firmware, d.comando_saida, d.id, ud.id_usuario, ud.nome, ud.id, ud.id_dispositivo FROM dispositivos d INNER JOIN usuarios_dispositivos ud ON d.id = ud.id_dispositivo WHERE ud.id_usuario = @idUsuario;";
                    comando.Parameters.AddWithValue("@idUsuario", IdUsuario);
                    
                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        List<DispositivoStatus> dispositivosStatus = new List<DispositivoStatus>();
                        while (reader.Read())
                        {
                            DispositivoStatus dispositivoStatus = new DispositivoStatus();
                            dispositivoStatus.Codigo = reader.GetString(0);
                            dispositivoStatus.TabelaDados = reader.GetString(1);
                            dispositivoStatus.VersaoFirmware = reader.GetDouble(2);
                            dispositivoStatus.ComandoSaida = reader.GetBoolean(3);
                            dispositivoStatus.IdDispositivo = reader.GetGuid(4);
                            dispositivoStatus.IdUsuario = reader.GetGuid(5);
                            dispositivoStatus.Nome = reader.GetString(6);
                            dispositivoStatus.IdUsuarioDispositivo = reader.GetGuid(7);
                            dispositivoStatus.IdDispositivo = reader.GetGuid(8);

                            dispositivoStatus.Online = VerificaDispositivoOnline(dispositivoStatus.TabelaDados);

                            DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
                            dadosLocalizacaoDispositivo = UltimoDadoRecebido(dispositivoStatus.TabelaDados);
                            dispositivoStatus.UltimoDadoRecebido = dadosLocalizacaoDispositivo.DataHora;
                        
                            dispositivosStatus.Add(dispositivoStatus);
                        }
                        return dispositivosStatus;
                    }
                }
            }
        }

        public List<DadosLocalizacaoDispositivo> BuscaDadosLocalizacaoDispositivos(string TabelaDados, DateTime dataInicio, DateTime dataFim)
        {
            using (var con = new Npgsql.NpgsqlConnection(connectionString))
            {
                con.Open();
                using (var comando = new NpgsqlCommand())
                {
                    comando.Connection = con;
                    comando.CommandText = "SELECT data_hora, latitude, longitude, altitude, satelites, sinal_operadora, saida FROM " + TabelaDados + " WHERE data_hora >= @DataHoraInicio AND data_hora <= @DataHoraFim ORDER BY data_hora ASC;";
                    comando.Parameters.AddWithValue("@DataHoraInicio", dataInicio);
                    comando.Parameters.AddWithValue("@DataHoraFim", dataFim);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        List<DadosLocalizacaoDispositivo> dadosLocalizacaoDispositivos = new List<DadosLocalizacaoDispositivo>();
                        while (reader.Read())
                        {
                            DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo = new DadosLocalizacaoDispositivo();
                            dadosLocalizacaoDispositivo.DataHora = reader.GetDateTime(0).ToString("dd/MM/yy HH:mm:ss");
                            dadosLocalizacaoDispositivo.Latitude = reader.GetDouble(1);
                            dadosLocalizacaoDispositivo.Longitude = reader.GetDouble(2);
                            dadosLocalizacaoDispositivo.Altitude = reader.GetDouble(3);
                            dadosLocalizacaoDispositivo.Satelites = reader.GetDecimal(4);
                            dadosLocalizacaoDispositivo.SinalOperadora = reader.GetDecimal(5);
                            dadosLocalizacaoDispositivo.Saida = reader.GetBoolean(6);

                            dadosLocalizacaoDispositivos.Add(dadosLocalizacaoDispositivo);
                        }
                        return dadosLocalizacaoDispositivos;
                    }
                }
            }
        }

        public Dispositivo DadosDispositivo(Guid IdUsuarioDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT ud.id_usuario, ud.nome, ud.id, d.codigo, d.tabela_dados, d.versao_firmware, d.comando_saida, d.id  FROM public.usuarios_dispositivos ud INNER JOIN public.dispositivos d ON d.id = ud.id_dispositivo WHERE ud.id = @id_UsuarioDispositivo;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id_UsuarioDispositivo", IdUsuarioDispositivo);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        Dispositivo dispositivo = new Dispositivo();
                        if (reader.Read())
                        {
                            dispositivo.IdUsuario = reader.GetGuid(0);
                            dispositivo.Nome = reader.GetString(1);
                            dispositivo.IdUsuarioDispositivo = reader.GetGuid(2);
                            dispositivo.Codigo = reader.GetString(3);
                            dispositivo.TabelaDados = reader.GetString(4);
                            dispositivo.VersaoFirmware = reader.GetDouble(5);
                            dispositivo.ComandoSaida = reader.GetBoolean(6);
                            dispositivo.IdDispositivo = reader.GetGuid(7);
                        }
                   
                        return dispositivo;
                    }
                }
            }
        }

        public Dispositivo DadosDispositivo(string CodigoDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT codigo, tabela_dados, versao_firmware, comando_saida, id  FROM public.dispositivos WHERE codigo = @Codigo;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@Codigo", CodigoDispositivo);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        Dispositivo dispositivo = new Dispositivo();
                        if (reader.Read())
                        {                         
                            dispositivo.Codigo = reader.GetString(0);
                            dispositivo.TabelaDados = reader.GetString(1);
                            dispositivo.VersaoFirmware = reader.GetDouble(2);
                            dispositivo.ComandoSaida = reader.GetBoolean(3);
                            dispositivo.IdDispositivo = reader.GetGuid(4);
                        }

                        return dispositivo;
                    }
                }
            }
        }


        public Decimal QuantidadeDados(string TabelaDados)
        {
            using (var con = new Npgsql.NpgsqlConnection(connectionString))
            {
                con.Open();
                using (var comando = new NpgsqlCommand())
                {                
                    comando.Connection = con;
                    comando.CommandText = "SELECT COUNT(*) FROM "+ TabelaDados + ";";

                    // Execute a consulta e obtenha o resultado
                    long quantidadeLinhas = (long)comando.ExecuteScalar();

                    return quantidadeLinhas;
                }
            }

        }


        // Salva dados Localizacao Dispositivo
        public bool SalvaLocalizacaoDispositivo(string TabelaDados, DadosLocalizacaoDispositivo dadosLocalizacaoDispositivo)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "INSERT INTO public."+ TabelaDados + " (data_hora, latitude, longitude, altitude, sinal_operadora, satelites, saida) VALUES('"+ dadosLocalizacaoDispositivo.DataHora + "', @latitude, @longitude, @altitude, @sinal_operadora, @satelites, @saida)";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {

                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@latitude", dadosLocalizacaoDispositivo.Latitude);
                    comando.Parameters.AddWithValue("@longitude", dadosLocalizacaoDispositivo.Longitude);
                    comando.Parameters.AddWithValue("@altitude", dadosLocalizacaoDispositivo.Altitude);
                    comando.Parameters.AddWithValue("@sinal_operadora", dadosLocalizacaoDispositivo.SinalOperadora);
                    comando.Parameters.AddWithValue("@satelites", dadosLocalizacaoDispositivo.Satelites);
                    comando.Parameters.AddWithValue("@saida", dadosLocalizacaoDispositivo.Saida);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }       
        }
    
        public bool UpdateVersaoDispositivo(Guid IdDispositivo, double Versao)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "UPDATE public.dispositivos SET versao_firmware=@Versao  WHERE id=@id";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {

                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@Versao", Versao);
                    comando.Parameters.AddWithValue("@id", IdDispositivo);
                 
                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
        

    }
}
