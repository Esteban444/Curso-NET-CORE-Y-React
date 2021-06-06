using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            public readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
               _instructorRepository = instructor;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _instructorRepository.ObtenerPorId(request.Id);
                if(instructor == null){
                    throw new ManejoExcepciones(HttpStatusCode.NotFound, new {mensaje = "El instructor no existe en la base de datos."});
                }
                return instructor;
            }
        }
    }
}