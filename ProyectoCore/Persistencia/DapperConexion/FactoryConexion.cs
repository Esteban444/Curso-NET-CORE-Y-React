using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persistencia.DapperConecxion;

namespace Persistencia.DapperConexion
{
    public class FactoryConexion : IFactoryConection
    {
        private IDbConnection _connection;
        private readonly IOptions<ConfiguracionConexion> _config;
        public FactoryConexion(IOptions<ConfiguracionConexion> config)
        {
              _config = config;
        }
        public void CloseConexion()
        {
            if(_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public IDbConnection GetConexion()
        {
          if(_connection == null)
            {
                _connection = new SqlConnection(_config.Value.DefaultConnection);
            }
          if (_connection.State != ConnectionState.Open)
            {
               _connection.Open();
            }
           return _connection;
        }
    }
}