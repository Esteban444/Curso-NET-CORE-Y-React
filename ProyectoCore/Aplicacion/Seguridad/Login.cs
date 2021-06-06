using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class EjecutaL :IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<EjecutaL>
        {
           public EjecutaValidacion()
           {
               RuleFor(c => c.Email).NotEmpty().WithMessage("El campo email no puede ir vacío");
               RuleFor(c => c.Password).NotEmpty().WithMessage("El campo contraseña no puede ir vacío");
           }
        }
        public class Manejador : IRequestHandler<EjecutaL, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IjwtGenerador _jwtGenerador;
            public Manejador (UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IjwtGenerador jwtGenerador)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(EjecutaL request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if(usuario == null)
                {
                    throw new ManejoExcepciones(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);//agregado para solucionar error cuando se agregan los roles en los claim de jwtGenerador
                var listaRoles = new List<string>(resultadoRoles);
                if(resultado.Succeeded)
                {
                    return  new UsuarioData
                    {
                       NombreCompleto = usuario.NombreCompleto,
                       Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                       UserName = usuario.UserName,
                       Email = usuario.Email,
                       Imagen = null
                    };
                }
                throw new ManejoExcepciones(HttpStatusCode.Unauthorized);
            }
        }
    }
}