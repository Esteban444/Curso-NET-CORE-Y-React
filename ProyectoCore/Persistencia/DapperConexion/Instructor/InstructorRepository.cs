using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConection _FactoryConection;

        public InstructorRepository(IFactoryConection factory)
        {
            _FactoryConection = factory;
        }
        public async Task<int> Actualizar(Guid instructorId, string nombre, string apellidos, string grado)
        {
            var storeProcedure = "usp_Actualizar_Instructor";
            try
            {
                var conexion = _FactoryConection.GetConexion();
                var resultado = await conexion.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        InstructorId = instructorId,
                        Nombre = nombre,
                        Apellidos = apellidos,
                        Grado = grado
                    },
                    commandType: CommandType.StoredProcedure

                );
                _FactoryConection.CloseConexion();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo editar la data del instructor", e);
            }
        }

        public async Task<int> Eliminar(Guid id)
        {
            var storeProcedure = "usp_Eliminar_Instructor";
            try
            {
                var conexion = _FactoryConection.GetConexion();
                var resultado = await conexion.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        InstructorId = id
                    },
                    commandType: CommandType.StoredProcedure

                );
                _FactoryConection.CloseConexion();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo eliminar el instructor", e);
            }
        }

        public async Task<int> nuevo(string Nombre, string Apellidos, string Grado)
        {
            var storeProcedure = "usp_Insertar_Instructor";
            try
            {
                var conexion = _FactoryConection.GetConexion();
                var resultado = await conexion.ExecuteAsync(
                storeProcedure,
                new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = Nombre,
                    Apellidos = Apellidos,
                    Grado = Grado
                },
                commandType: CommandType.StoredProcedure
                );
                _FactoryConection.CloseConexion();

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }

        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorlist = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var conexion = _FactoryConection.GetConexion();
                instructorlist = await conexion.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                _FactoryConection.CloseConexion();
            }
            return instructorlist;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_Obtener_InstructorPorId";
            InstructorModel instructor = null;
            try
            {
                var conexion = _FactoryConection.GetConexion();
                instructor =  await conexion.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new 
                    { 
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure
                );
                return instructor;
            }
            catch (Exception e)
            {
               throw new Exception("No se pudo eliminar el instructor", e);
            }
        }
    }
}