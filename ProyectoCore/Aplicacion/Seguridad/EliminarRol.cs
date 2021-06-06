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
    public class EliminarRol
    {
        public class EjecutaERol: IRequest
        {
            public string Nombre { get; set; }
        }

        public class EjecutaValidator:AbstractValidator<EjecutaERol>
        {
           public EjecutaValidator()
           {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El campo Nombre no puede ir vacío");
           }
        }

        public class ManejadorEliR : IRequestHandler<EjecutaERol>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public ManejadorEliR(RoleManager<IdentityRole> role)
            {
                _roleManager = role;
            }
            public async Task<Unit> Handle(EjecutaERol request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Nombre);
                if(role == null){
                    throw new ManejoExcepciones(HttpStatusCode.BadRequest, new {mensaje = "No existe el role" });
                }
                var resultado = await _roleManager.DeleteAsync(role);
                if(resultado.Succeeded){
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el role");
            }
        }
    }
}