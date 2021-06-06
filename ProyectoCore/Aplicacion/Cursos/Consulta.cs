using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>>{}

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly IMapper _imapper;
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context,IMapper mapper )
            {
                 _context = context;
                 _imapper = mapper;
            }
            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Cursos
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x => x.Instructoreslink).ThenInclude(x => x.Instructor).ToListAsync();
                var cursosDto = _imapper.Map<List<Curso>, List<CursoDTO>>(cursos);
                return cursosDto;
            }
        }
    }
}