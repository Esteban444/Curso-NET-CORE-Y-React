using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class EjecutaR :IRequest<UsuarioData>{
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }
        public class EjecutaValidador: AbstractValidator<EjecutaR>
        {
           public EjecutaValidador()
           {
              RuleFor(c => c.Nombre).NotEmpty().WithMessage("El campo Nombre no puede ir vacío");
              RuleFor(c => c.Apellidos).NotEmpty().WithMessage("El campo Apellidos no puede ir vacío");
              RuleFor(c => c.Email).NotEmpty().WithMessage("El campo Email no puede ir vacío");
              RuleFor(c => c.Password).NotEmpty().WithMessage("El campo Password no puede ir vacío");
              RuleFor(c => c.UserName).NotEmpty().WithMessage("El campo UserName no puede ir vacío");
           }
        }
        public class Manejador : IRequestHandler<EjecutaR, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IjwtGenerador _jwtgenerador;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IjwtGenerador jwtgenerador)
            {
               _context = context;
               _userManager = userManager;
               _jwtgenerador = jwtgenerador;
            }
            public async Task<UsuarioData> Handle(EjecutaR request, CancellationToken cancellationToken)
            {
                var existe =  await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    throw new ManejoExcepciones(HttpStatusCode.BadRequest, new {mensae = "Existe ya un usuario registrado con ese email"});
                }
                 
                 var exiteUserName = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
                 if (exiteUserName)
                 {
                    throw new ManejoExcepciones(HttpStatusCode.BadRequest, new {mensae = "El nombre de usuario ya existe en la base de datos"});
                 }
                var usuario = new Usuario
                {
                     NombreCompleto = request.Nombre + " " + request.Apellidos,
                     Email = request.Email,
                     UserName = request.UserName

                };

                 var resultado = await _userManager.CreateAsync(usuario, request.Password);
                 if(resultado.Succeeded)
                 {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtgenerador.CrearToken(usuario, null),// El null se para solucionar error cuando se agregan los roles en los claim de jwtGenerador
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                 }

                 throw new Exception("No se pudo agregar el nuevo usuario");
            }
        }
    }
}