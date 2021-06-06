using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RolController: MiControllerBase
    {
        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Lista()
        {
            return await Mediator.Send(new ListaRoles.EjecutaRl());
        }

        [HttpGet("{UserName}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesPorUsuario(string UserName)
        {
           return await Mediator.Send(new ObtenerRolesPorUsuario.EjecutaObtRolesUsua{UserName = UserName});
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> CrearRol(RolNuevo.EjecutaRol parametros)
        {
               return await Mediator.Send(parametros);
        }

        [HttpPost("agregarRolUsuario")]
         
         public async Task<ActionResult<Unit>> AgregarRolUsuario(AgregarRolUsuario.EjecutaRlA parametros)
         {
            return  await Mediator.Send(parametros);
         }

        [HttpPost("EliminarRolUsuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario(EliminarUsuarioRol.EjecutaERolUsuario parametros)
        {
               return await Mediator.Send(parametros);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> EliminarRol(EliminarRol.EjecutaERol parametros)
        {
                     return await Mediator.Send(parametros);
        }
    }
}