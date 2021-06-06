using System.Collections.Generic;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionModel
    {
        public List<IDictionary<string, object>> ListaRecords { get; set; }
        //Retornar un arreglo de tipo json
        public int TotalRecords { get; set; }
        public int NumeroPaginas { get; set; }
    }
}