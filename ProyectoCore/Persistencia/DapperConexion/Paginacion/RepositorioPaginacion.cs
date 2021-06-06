using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class RepositorioPaginacion : IPaginacion
    {
        private readonly IFactoryConection _factoryConection;
        public RepositorioPaginacion(IFactoryConection factoryConection)
        {
            _factoryConection = factoryConection;
        }
        public async Task<PaginacionModel> DevolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> FiltroParametros, string ordenamientoColumna)
        {
            PaginacionModel  paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try
            {
                 var conexion = _factoryConection.GetConexion();
                 DynamicParameters parameters = new DynamicParameters();

                  foreach(var param in FiltroParametros)
                  {
                     parameters.Add("@" + param.Key, param.Value);
                  }

                 parameters.Add("@NumeroPagina", numeroPagina);// parametros de entrada
                 parameters.Add("@CantidadElementos", cantidadElementos);
                 parameters.Add("@Ordenamiento", ordenamientoColumna);

                 //parametros de salida
                  parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                  parameters.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);



                 var result = await conexion.QueryAsync(storeProcedure, parameters,commandType: CommandType.StoredProcedure);
                 listaReporte = result.Select(x => (IDictionary<string, object>)x).ToList();
                 paginacionModel.ListaRecords = listaReporte;
                 paginacionModel.NumeroPaginas = parameters.Get<int>("@TotalPaginas");
                 paginacionModel.TotalRecords = parameters.Get<int>("@TotalRecords");
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo ejecutar el procedimiento almacenado",e);
            }
            finally
            {
                 _factoryConection.CloseConexion();
            }
            return paginacionModel;
        }
    }
}