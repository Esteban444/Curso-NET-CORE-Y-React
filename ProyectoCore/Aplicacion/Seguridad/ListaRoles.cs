using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class ListaRoles
    {
        public class EjecutaRl:IRequest<List<IdentityRole>>{}

        public class MenejadorObt : IRequestHandler<EjecutaRl, List<IdentityRole>>
        {
            private readonly CursosOnlineContext _context;
            public MenejadorObt(CursosOnlineContext context)
            {
               _context = context;
            }
            public async Task<List<IdentityRole>> Handle(EjecutaRl request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync();
                return roles;
            }
        }
    }
}