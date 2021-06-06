using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar: IRequest<UsuarioData>{}

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IjwtGenerador _jwtgenerador;
            private readonly IUsuarioSesion _UsuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IjwtGenerador jwtgenerador, IUsuarioSesion usuariosesion )
            {
                _userManager = userManager;
                _jwtgenerador = jwtgenerador;
                _UsuarioSesion = usuariosesion;
            }
            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
               var usuario =  await _userManager.FindByNameAsync(_UsuarioSesion.ObtenerUsuarioSesion());
               var resultadoRoles = await _userManager.GetRolesAsync(usuario);//agregado para solucionar error cuando se agregan los roles en los claim de jwtGenerador
               var listaRoles = new List<string>(resultadoRoles);
              return new UsuarioData
               {
                 NombreCompleto = usuario.NombreCompleto,
                 UserName = usuario.UserName,
                 Token = _jwtgenerador.CrearToken(usuario, listaRoles),
                 Imagen = null,
                 Email = usuario.Email
               };
            }
        }
    }
}