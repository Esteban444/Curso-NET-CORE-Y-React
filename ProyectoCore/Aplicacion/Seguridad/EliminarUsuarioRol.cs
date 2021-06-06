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
    public class EliminarUsuarioRol
    {
        public class EjecutaERolUsuario: IRequest{
            public string UserName { get; set; }
            public string RolNombre { get; set;}
        }

        public class EjecutaValidator : AbstractValidator<EjecutaERolUsuario>
        {
            public EjecutaValidator()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage("El campo UserName no puede ir vacío");
                RuleFor(x => x.RolNombre).NotEmpty().WithMessage("El campo RolNombre no puede ir vacío");
            }
        }
        public class ManejadorEliRolUsu : IRequestHandler<EjecutaERolUsuario>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _rolesManager;
            public ManejadorEliRolUsu(UserManager<Usuario> userManager, RoleManager<IdentityRole> rolesManager)
            {
                 _userManager = userManager;
                _rolesManager = rolesManager;
            }
            public async Task<Unit> Handle(EjecutaERolUsuario request, CancellationToken cancellationToken)
            {
                var role = await _rolesManager.FindByNameAsync(request.RolNombre);
                if(role == null){
                     throw new ManejoExcepciones(HttpStatusCode.NotFound, new {messaje = "No se encontro el rol"});
                }
                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if(usuario == null){
                    throw new ManejoExcepciones(HttpStatusCode.NotFound, new {messaje = "El usuario no existe"});
                }
                var resultado = await _userManager.RemoveFromRoleAsync(usuario, request.RolNombre);
                if(resultado.Succeeded){
                  return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el rol al usurio");
            }
        }
    }
}