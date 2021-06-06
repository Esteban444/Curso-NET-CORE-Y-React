using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
         Task<IEnumerable<InstructorModel>> ObtenerLista();
         Task<InstructorModel> ObtenerPorId(Guid id);
         Task<int> nuevo(string Nombre, string Apellidos, string Grado);
         Task<int> Actualizar(Guid InstructorId, string Nombre, string Apellidos, string Grado);
         Task<int> Eliminar(Guid id);
    }
}