using ApiPetShop.Domain.Models;
using ApiPetShop.Filters.Exceptions;
using ApiPetShop.Repositories.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShop.Repositories.Repositories
{
    // Herdo parametros da Data/Context
    public class ClienteRepository : Context
    {
        // função para inserção de dados no Banco de dados
        public void Insert(ClienteModel model)
        {
            // crio uma variavel que recebe o parametro de insert do banco de dados
            string commandSql = @"INSERT INTO Cliente 
                                    (CpfCliente, Nome, Nascimento, Telefone) 
                                  VALUES
                                    (@CpfCliente, @Nome, @Nascimento, @Telefone);";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser iserido
            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                // injeto os parametros para a inserção no banco e executo sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@CpfCliente", model.CpfCliente);
                cmd.Parameters.AddWithValue("@Nome", model.Nome);
                cmd.Parameters.AddWithValue("@Nascimento", model.Nascimento);
                cmd.Parameters.AddWithValue("@Telefone", model.Telefone);
                cmd.ExecuteNonQuery();
            }
        }

        // função para atualização de dados no Banco de dados
        public void Update(ClienteModel model)
        {
            // crio uma variavel que recebe o parametro de update do banco, passando o CPF do cliente
            string commandSql = @"UPDATE Cliente 
                                  SET 
                                    Nome = @nome, Nascimento = @nascimento, Telefone = @telefone 
                                  WHERE 
                                    CpfCliente = @CpfCliente;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser iserido
            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                // injeto os parametros para a inserção no banco e executo sem nenhum tipo de retorno
                cmd.Parameters.AddWithValue("@CpfCliente", model.CpfCliente);
                cmd.Parameters.AddWithValue("@Nome", model.Nome);
                cmd.Parameters.AddWithValue("@Nascimento", model.Nascimento);
                cmd.Parameters.AddWithValue("@Telefone", model.Telefone);

                // faco uma condição, que se não houve nenhum dado afetado, estoura uma exceção. Informando que não houve alteração
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidationException($"Nenhum registro afetado para o cpf {model.CpfCliente}");
            }
        }

        // função para listar todos os clientes do banco de dados, podendo ou não receber um valor
        public List<ClienteModel> GetAll(string? nome)
        {
            // crio uma variavel que recebe a lista de todos os cadastros existentes no banco
            string commandSql = @"SELECT CpfCliente, Nome, Nascimento, Telefone FROM Cliente";

            // caso tenha valor na variavel adiciono a pesquisa seletiva
            if (!string.IsNullOrWhiteSpace(nome))
                commandSql += " WHERE nome LIKE @nome";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser localizado, caso houver
            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                // verfico se o valor é nulo ou tem espaços
                if (!string.IsNullOrWhiteSpace(nome))
                    cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                // crio uma variavel reader para percorrer pelo banco gerando uma lista de cliente, retornando a lista
                using (var rdr = cmd.ExecuteReader())
                {
                    var clientes = new List<ClienteModel>();
                    while (rdr.Read())
                    {
                        var cliente = new ClienteModel();
                        cliente.CpfCliente = Convert.ToString(rdr["CpfCliente"]);
                        cliente.Nome = Convert.ToString(rdr["Nome"]);
                        cliente.Nascimento = Convert.ToDateTime(rdr["Nascimento"]);
                        cliente.Telefone = rdr["Telefone"] == DBNull.Value ? null : Convert.ToString(rdr["Telefone"]);
                        clientes.Add(cliente);
                    }
                    return clientes;
                }
            }
        }

        // função para deletar um cliente do banco de dados
        public void Delete(string cpfCliente)
        {
            // crio uma variavel que recebe o parametro de deleção
            string commandSql = @"DELETE FROM Cliente 
                                  WHERE 
                                    CpfCliente = @CpfCliente;";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser deletado
            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@CpfCliente", cpfCliente);

                // realizo tratativa, caso não tenha afetado nenhum cliente da base, retorna que nenhum registro foi alterado
                if (cmd.ExecuteNonQuery() == 0)
                    throw new ValidationException($"Nenhum registro afetado para o cpf {cpfCliente}");
            }
        }

        // função para buscar se o cadastro já existe, antes de inserir
        public bool Exist(string cpfCliente)
        {
            // crio uma variavel que recebe a lista de todos os cadastros existentes no banco
            string commandSql = @"SELECT CpfCliente, Nome, Nascimento, Telefone FROM Cliente";

            // realizo um using, criando uma variavel recebendo o comando do sql e o modelo de dados a ser deletado
            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@CpfCliente", cpfCliente);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        // função para buscar cliente específico
        public ClienteModel? GetSpecific(string cpfCliente)
        {
            string commandSql = @"SELECT CpfCliente, Nome, Nascimento, Telefone FROM Cliente WHERE CpfCliente = @CpfCliente";

            using (var cmd = new MySqlCommand(commandSql, _conn))
            {
                cmd.Parameters.AddWithValue("@CpfCliente", cpfCliente);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var cliente = new ClienteModel();
                        cliente.CpfCliente = Convert.ToString(rdr["CpfCliente"]);
                        cliente.Nome = Convert.ToString(rdr["Nome"]);
                        cliente.Nascimento = Convert.ToDateTime(rdr["Nascimento"]);
                        cliente.Telefone = rdr["Telefone"] == DBNull.Value ? null : Convert.ToString(rdr["Telefone"]);
                        return cliente;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
