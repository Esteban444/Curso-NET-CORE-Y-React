using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class NuevoCurso
    {
        public class Ejecutar: IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? Fechapublicacion { get; set; }

            public List<Guid> ListaInstrutor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }
        }

        public class EjecutaValidacion: AbstractValidator<Ejecutar>
        {
            public EjecutaValidacion()
            {
             RuleFor(c => c.Titulo).NotEmpty().WithMessage("El campo Título no puede ir vacío");
             RuleFor(c => c.Descripcion).NotEmpty().WithMessage("El campo Descripción no puede ir vacío");
             RuleFor(c => c.Fechapublicacion).NotEmpty().WithMessage("El campo FechaPublicación no puede ir vacío");
            }
        }

        public class Manejador : IRequestHandler<Ejecutar>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                Guid _CursorId = Guid.NewGuid();
                var curso = new Curso{
                    CursoId = _CursorId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    Fechapublicacion = request.Fechapublicacion,
                    FechaCreacion = DateTime.UtcNow // Frecha creacion automatica
                };
                _context.Cursos.Add(curso);
                // LOgica para agregar los instructores
                if(request.ListaInstrutor != null)
                {
                    foreach(var id in request.ListaInstrutor)
                    {
                        var cursoInstructor = new CursoInstructor
                       {
                          CursoId = _CursorId,
                          InstructorId = id
                       };
                       _context.CursoInstructor.Add(cursoInstructor);
                    }

                }

                // Logica para agregar el precio del curso

                var precioentidad = new Precio
                {
                     CursoId = _CursorId,
                     PrecioActual = request.Precio,
                     Promocion = request.Promocion,
                     PrecioId = Guid.NewGuid()
                };
                 _context.Precios.Add(precioentidad);

                var valor = await _context.SaveChangesAsync();
                if(valor > 0){
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}