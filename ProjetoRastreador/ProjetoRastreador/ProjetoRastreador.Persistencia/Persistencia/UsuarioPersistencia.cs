
using Npgsql;
using ProjetoRastreador.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoRastreador.Persistencia.Persistencia
{
    public class UsuarioPersistencia
    {
        private string connectionString = "Server = localhost; Port = 5432; Database = rastreador; User Id = postgres; Password = (!-!1&L&;";

        public UsuarioPersistencia() { }

        public Guid LoginUsuario(Usuario usuario)
        {      
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                // consulta SQL INSERT
                string sql = "SELECT id FROM usuarios WHERE email = @Email AND senha = @Senha;";

                conexao.Open();

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
   
                    comando.Parameters.AddWithValue("Email", usuario.Email);
                    comando.Parameters.AddWithValue("Senha", usuario.Senha);

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

        public Usuario BuscaUsuario(Guid id)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                string sql = "SELECT * FROM usuarios WHERE id = @id;";

                conexao.Open();
                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (NpgsqlDataReader reader = comando.ExecuteReader())
                    {
                        Usuario usuario = new Usuario();

                        if (reader.Read())
                        {
                            usuario.Id = reader.GetGuid(0);
                            usuario.Nome = reader.GetString(1);
                            usuario.Fone = reader.GetString(2);
                            usuario.Email = reader.GetString(3);           
                            usuario.Senha = reader.GetString(4);
                        }

                        return usuario;
                    }
                }
            }
        }

        public Guid NovoUsuario(Usuario usuario)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "INSERT INTO public.usuarios (id, nome, fone, email, senha) VALUES(@id, @nome, @fone, @email, @senha)";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {                  
                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@id", usuario.Id);
                    comando.Parameters.AddWithValue("@nome", usuario.Nome);
                    comando.Parameters.AddWithValue("@fone", usuario.Fone);
                    comando.Parameters.AddWithValue("@email", usuario.Email);
                    comando.Parameters.AddWithValue("@senha", usuario.Senha);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();
                }                
            }
            return usuario.Id;
        }

        public Guid VerificaExisteEmail(Usuario usuario)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "SELECT id FROM usuarios WHERE email = @email;";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@email", usuario.Email);

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
        public bool SalvaUsuario(Usuario usuario)
        {
            using (var conexao = new Npgsql.NpgsqlConnection(connectionString))
            {
                conexao.Open();

                // consulta SQL INSERT
                string sql = "UPDATE public.usuarios SET nome=@nome, fone=@fone, email=@email, senha=@senha WHERE id=@id";

                // cria um comando SQL
                using (NpgsqlCommand comando = new NpgsqlCommand(sql, conexao))
                {
                    // adiciona os parâmetros à consulta SQL
                    comando.Parameters.AddWithValue("@id", usuario.Id);
                    comando.Parameters.AddWithValue("@nome", usuario.Nome);
                    comando.Parameters.AddWithValue("@fone", usuario.Fone);
                    comando.Parameters.AddWithValue("@email", usuario.Email);
                    comando.Parameters.AddWithValue("@senha", usuario.Senha);

                    // executa o comando SQL
                    int rowsAffected = comando.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
