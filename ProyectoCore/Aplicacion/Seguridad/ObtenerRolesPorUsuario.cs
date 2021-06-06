using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class EjecutaObtRolesUsua:IRequest<List<string>>
        {
            public string UserName { get; set; }
        }

        public class ManejadorObtenerRolesUsua : IRequestHandler<EjecutaObtRolesUsua, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _rolesManager;
            public ManejadorObtenerRolesUsua(UserManager<Usuario> userManager, RoleManager<IdentityRole> rolesManager)
            {
                _userManager = userManager;
                _rolesManager = rolesManager;
            }
            public async Task<List<string>> Handle(EjecutaObtRolesUsua request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if(usuario == null){
                  throw new ManejoExcepciones(HttpStatusCode.NotFound, new {messaje = "El usuario no existe"});
                }
                var resultado = await _userManager.GetRolesAsync(usuario);
                return new List<string>(resultado);
            }
        }
    }
}