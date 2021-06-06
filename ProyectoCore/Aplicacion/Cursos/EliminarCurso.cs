using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class EliminarCurso
    {
        public class Ejecutar: IRequest{
            public Guid Id { get; set; }
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
                var instructoresBd = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach (var instructor in instructoresBd)
                {
                    _context.CursoInstructor.Remove(instructor);

                }

                var comentariobd = _context.Comentarios.Where(x => x.CursoId == request.Id);
                foreach (var comentario in comentariobd){
                    _context.Comentarios.Remove(comentario);
                }

                var precioDb = _context.Precios.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if(precioDb != null)
                {
                    _context.Precios.Remove(precioDb);
                }
                
                var curso = await _context.Cursos.FindAsync(request.Id);
                if(curso == null){
                    //throw new Exception("El curso no existe en la base de datos");
                    throw new ManejoExcepciones(HttpStatusCode.NotFound,new {Mensaje = "El curso no existe en la base de datos"});
                }
                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();
                if(resultado > 0){
                    return Unit.Value;
                }
                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}