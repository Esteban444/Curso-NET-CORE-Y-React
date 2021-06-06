using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class AgregarRolUsuario
    {
        public class EjecutaRlA: IRequest
        {
            public string UserName { get; set; }
            public string RolNombre { get; set;}
        }
        public class EjecutaValidator : AbstractValidator<EjecutaRlA>
        {
              public EjecutaValidator()
              {
                 RuleFor(x => x.UserName).NotEmpty().WithMessage("El campo UserName no puede ir vacío");
                 RuleFor(x => x.RolNombre).NotEmpty().WithMessage("El campo RolNombre no puede ir vacío");
              }
        }

        public class ManejadorRLA : IRequestHandler<EjecutaRlA>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _rolesManager;

            public ManejadorRLA(UserManager<Usuario> userManager, RoleManager<IdentityRole> rolesManager)
            {
                 _userManager = userManager;
                 _rolesManager = rolesManager;
            }
            public async Task<Unit> Handle(EjecutaRlA request, CancellationToken cancellationToken)
            {
                var role = await _rolesManager.FindByNameAsync(request.RolNombre);
                if(role == null){
                    throw new ManejoExcepciones(HttpStatusCode.NotFound, new {message = "El rol no existe"});
                }
                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if(usuario == null){
                    throw new ManejoExcepciones(HttpStatusCode.NotFound, new {message = "El usuario no existe"});
                }
                var resultado = await _userManager.AddToRoleAsync(usuario, request.RolNombre);
                if(resultado.Succeeded){
                    return Unit.Value;
                }
                throw new Exception("No se pudo agregar el rol al usurio");
            }
        }
    }
}