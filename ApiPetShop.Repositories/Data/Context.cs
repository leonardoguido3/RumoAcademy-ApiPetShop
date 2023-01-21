using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPetShop.Repositories.Data
{
    public class Context
    {
        // injeto a dependencia do Mysql Server
        internal readonly MySqlConnection _conn;

        // construtor passando a injeção
        public Context()
        {
            // recebo como valor a minha conexão com banco de dados
            _conn = new MySqlConnection("Server=sql10.freemysqlhosting.net;Database=sql10591768;Uid=sql10591768;Pwd=tKTGmeezHq;");
        }

        // função para abertura da conexão com o banco de dados
        public void OpenConnection() { _conn.Open(); }

        // função para fechar a conexão com o banco de dados
        public void CloseConnection() { _conn.Close(); }
    }
}
