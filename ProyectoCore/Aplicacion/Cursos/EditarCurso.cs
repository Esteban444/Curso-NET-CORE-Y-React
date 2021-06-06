using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class EditarCurso
    {
        public class EjecutarE : IRequest{
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? Fechapublicacion { get; set; }

            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }
         
         public class EjecutaValidacion: AbstractValidator<EjecutarE>
{
    public EjecutaValidacion()
    {
     RuleFor(c => c.Titulo).NotEmpty().WithMessage("El campo Título no puede ir vacío");
     RuleFor(c => c.Descripcion).NotEmpty().WithMessage("El campo Descripción no puede ir vacío");
     RuleFor(c => c.Fechapublicacion).NotEmpty().WithMessage("El campo FechaPublicación no puede ir vacío");
    }
}
        public class Manejador : IRequestHandler<EjecutarE>
        {
            private readonly CursosOnlineContext _contex;
            public Manejador(CursosOnlineContext contex)
            {
                _contex = contex;
            }
            public async Task<Unit> Handle(EjecutarE request, CancellationToken cancellationToken)
            {
                var curso = await _contex.Cursos.FindAsync(request.CursoId);
                if(curso == null)
                {
                      throw new ManejoExcepciones(HttpStatusCode.NotFound,new {Mensaje = "El curso no existe en la base de datos"});
                }
                curso.Titulo = request.Titulo ?? curso.Titulo;//despues de  ?? es para si titulo es nulo que mantenga el titulo de antes.
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.Fechapublicacion = request.Fechapublicacion ?? curso.Fechapublicacion;
                curso.FechaCreacion = DateTime.UtcNow; //Fecha creacion automatica
                /*Logica actulizar precio del curso*/
                var precioentidad = _contex.Precios.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioentidad != null)
                {
                   precioentidad.PrecioActual = request.Precio ?? precioentidad.PrecioActual;
                   precioentidad.Promocion = request.Promocion ?? precioentidad.Promocion;
                }else{
                    precioentidad = new Precio{
                      PrecioId = Guid.NewGuid(),
                      PrecioActual = request.Precio ?? 0,
                      Promocion = request.Promocion ?? 0,
                      CursoId =  curso.CursoId
                    };
                    await _contex.Precios.AddAsync(precioentidad);
                }

                if(request.ListaInstructor != null)
                {
                   if(request.ListaInstructor.Count > 0)
                   {
                       //Eliminar los instructores Actuales del curso en la base de datos 
                       var instructoresDb = _contex.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                       foreach (var instructorEliminar in instructoresDb)
                       {
                           _contex.CursoInstructor.Remove(instructorEliminar);
                       }
                       //fin de procedimiento eliminar instructores

                       /*Procedimiento para Agregar instructores que provienen del cliente*/
                       foreach (var id in request.ListaInstructor)
                       {
                           var nuevoInstructor = new CursoInstructor()
                           {
                               CursoId = request.CursoId,
                               InstructorId = id
                           };
                           _contex.CursoInstructor.Add(nuevoInstructor);
                       }
                       /*Fin procedimiento*/
                   }
                }
                var resultado = await _contex.SaveChangesAsync();
                if(resultado > 0)
                     return Unit.Value;
                     throw new Exception("No se guardaron los Cambios");
            }
        }
    }
}