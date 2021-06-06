using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico: IRequest<CursoDTO>{
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly IMapper _mapper;
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x => x.Instructoreslink).
                ThenInclude(y => y.Instructor).FirstOrDefaultAsync(c=> c.CursoId == request.Id);
                if(curso == null)
                {
                  throw new ManejoExcepciones(HttpStatusCode.NotFound,new {Mensaje = "El curso no existe en la base de datos"});
                }
                var cursoDto = _mapper.Map<Curso, CursoDTO>(curso);
                return cursoDto;
            }
        }
    }
}