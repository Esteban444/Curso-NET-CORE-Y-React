using System;
using System.Collections.Generic;
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
    public class UsuarioActualizar
    {
        public class EjecutaUsuaActualizar: IRequest<UsuarioData>
        {
           public string Nombre { get; set; }
           public string Apellidos { get; set; }
           public string Email { get; set; }
           public string Password { get; set; }

           public string UserName { get; set; }
        }
        
         public class EjecutaValidador: AbstractValidator<EjecutaUsuaActualizar>
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

        public class ManejadorUsuaActualizar : IRequestHandler<EjecutaUsuaActualizar, UsuarioData>
        {
            private readonly CursosOnlineContext _Context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IjwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _PasswordHasher;

            public ManejadorUsuaActualizar(CursosOnlineContext context, UserManager<Usuario> userManager, IjwtGenerador jwtGenerador,IPasswordHasher<Usuario> passwordHasher)
            {
                _Context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _PasswordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(EjecutaUsuaActualizar request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if(usuario == null){
                    throw new ManejoExcepciones(HttpStatusCode.NotFound, new {message = "El usuario no existe"});
                }
               var resultado = await _Context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();
               if(resultado){
                   throw new ManejoExcepciones(HttpStatusCode.InternalServerError, new {message ="Este imail pertenece a otro usuario"});
               }
               usuario.NombreCompleto = request.Nombre + " " + request.Apellidos;
               usuario.PasswordHash = _PasswordHasher.HashPassword(usuario, request.Password);
               usuario.Email = request.Email;

               var resultadoUp = await _userManager.UpdateAsync(usuario);

               var resultadoRoles = await _userManager.GetRolesAsync(usuario);
               var listRoles = new List<string>(resultadoRoles);

               if(resultadoUp.Succeeded)
               {
                   return new UsuarioData{
                       NombreCompleto = usuario.NombreCompleto,
                       UserName = usuario.UserName,
                       Email = usuario.Email,
                       Token = _jwtGenerador.CrearToken(usuario, listRoles)
                   };
               }
               throw new Exception("No se pudo Actualizar el usuario");
            }
        }
    }
}
