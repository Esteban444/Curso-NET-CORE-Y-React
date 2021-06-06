using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class EliminarInstructor
    {
        public class EjecutaE : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<EjecutaE>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
               _instructorRepository = instructor;
            }
            public async Task<Unit> Handle(EjecutaE request, CancellationToken cancellationToken)
            {
                 var resultado = await _instructorRepository.Eliminar(request.Id);
                 if(resultado > 0){
                     return Unit.Value;
                 }

                 throw new Exception("No se pudo eliminar el instructor");
            }
        }
    }
}