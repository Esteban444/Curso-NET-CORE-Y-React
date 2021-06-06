using System.Data;

namespace Persistencia.DapperConexion
{
    public interface IFactoryConection
    {
        void CloseConexion();
         IDbConnection GetConexion();
    }
}