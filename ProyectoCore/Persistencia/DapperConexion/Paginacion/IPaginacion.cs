using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
         Task<PaginacionModel> DevolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, 
         IDictionary<string, object> FiltroParametros, string ordenamientoColumna);
    }
}