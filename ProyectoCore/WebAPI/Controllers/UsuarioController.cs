using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
   [AllowAnonymous]
    public class UsuarioController: MiControllerBase
    {
       [HttpPost("login")]
       public async Task<ActionResult<UsuarioData>> Login(Login.EjecutaL parametros)
       {
          return await Mediator.Send(parametros);
       }

       [HttpPost("Registrar")]
       public async Task<ActionResult<UsuarioData>> RegistrarUsuario(Registrar.EjecutaR parametros)
       {
           return await Mediator.Send(parametros);
       }

       [HttpGet]

       public async Task<ActionResult<UsuarioData>> DevolverUsuario()
       {
          return  await Mediator.Send(new UsuarioActual.Ejecutar());
       }

       [HttpPut]
       public async Task<ActionResult<UsuarioData>>Actualizar(UsuarioActualizar.EjecutaUsuaActualizar parametros)
       {
            return await Mediator.Send(parametros);
       }
    }
}