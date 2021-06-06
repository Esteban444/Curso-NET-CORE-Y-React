using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class NuevoComentario
    {
        public class Ejecuta : IRequest{
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }

            public Guid CursoId { get; set; }
        }

        public class EjecutaValidator:AbstractValidator<Ejecuta>
        {
          public EjecutaValidator()
          {
               RuleFor(c => c.Alumno).NotEmpty().WithMessage("El campo Alumno no puede ir vacío");
               RuleFor(c => c.Puntaje).NotEmpty().WithMessage("El campo Puntaje no puede ir vacío");
               RuleFor(c => c.Comentario).NotEmpty().WithMessage("El campo Comentario no puede Comentario");
          }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _Context;
            public Manejador(CursosOnlineContext context)
            {
                _Context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = new Comentario{
                  ComentarioId = Guid.NewGuid(),
                   Alumno = request.Alumno,
                   Puntaje = request.Puntaje,
                   ComentarioTexto = request.Comentario,
                   CursoId = request.CursoId,
                   FechaCreacion = DateTime.UtcNow
                };

                _Context.Comentarios.Add(comentario);

                var resultado = await _Context.SaveChangesAsync();
                if (resultado > 0)
                {
                     return Unit.Value;
                }
                throw new Exception("No se pudo insertar el comentario");
            }
        }
    }
}