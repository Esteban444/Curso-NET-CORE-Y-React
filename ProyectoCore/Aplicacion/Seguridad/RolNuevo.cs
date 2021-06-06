using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class EjecutaRol: IRequest
        {
             public string Nombre { get; set; }
        }

        public class EjecutaValidator:AbstractValidator<EjecutaRol>
        {
           public EjecutaValidator()
           {
               RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo Nombre no puede ir vacío");
           }
        }

        public class ManejadorRol : IRequestHandler<EjecutaRol>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public ManejadorRol(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(EjecutaRol request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Nombre);
                if(role != null){
                    throw new ManejoExcepciones(HttpStatusCode.BadRequest, new {message = "Ya existe el rol"});
                }
                var resultado = await _roleManager.CreateAsync(new IdentityRole(request.Nombre));
                if(resultado.Succeeded){
                    return Unit.Value;
                }
                throw new Exception("No se pudo guardar el rol");
            }
        }
    }
}