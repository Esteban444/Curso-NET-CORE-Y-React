using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class NuevoInstructor
    {
        public class EjecutaI: IRequest{
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Grado { get; set; }
        }

        public class EjecutaValidator : AbstractValidator<EjecutaI>
        {
          public EjecutaValidator()
          {
              RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo Nombre no puede ir vacío");
              RuleFor(x => x.Apellidos).NotEmpty().WithMessage("El campo Apellidos no puede ir vacío");
              RuleFor(x => x.Grado).NotEmpty().WithMessage("El campo Grado no puede ir vacío");
          }
        }
        public class Manejador : IRequestHandler<EjecutaI>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructor)
            {
                _instructorRepository = instructor;
            }
            public async Task<Unit> Handle(EjecutaI request, CancellationToken cancellationToken)
            {
                var resultado =  await _instructorRepository.nuevo(request.Nombre, request.Apellidos, request.Grado);
                if (resultado > 0){
                    return Unit.Value;
                }
               throw new Exception("No se pudo importar el instructor");
            }
        }
    }
}