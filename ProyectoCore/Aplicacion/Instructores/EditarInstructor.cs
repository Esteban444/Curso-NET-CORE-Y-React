using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class EditarInstructor
    {
        public class EjecutaIE : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
        }

        public class EjecutaValidator : AbstractValidator<EjecutaIE>
        {
            public EjecutaValidator()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo Nombre no puede ir vacío");
                RuleFor(x => x.Apellidos).NotEmpty().WithMessage("El campo Apellidos no puede ir vacío");
                RuleFor(x => x.Grado).NotEmpty().WithMessage("El campo Grado no puede ir vacío");
            }
        }

        public class Manejador : IRequestHandler<EjecutaIE>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
                _instructorRepository = instructor;
            }
            public async Task<Unit> Handle(EjecutaIE request, CancellationToken cancellationToken)
            {
                var resultado = await _instructorRepository.Actualizar(request.InstructorId, request.Nombre, request.Apellidos, request.Grado);
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo actualizar el instructor");
            }
        }
    }
}