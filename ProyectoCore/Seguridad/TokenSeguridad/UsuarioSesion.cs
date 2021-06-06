using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Seguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _accessor;
        public UsuarioSesion(IHttpContextAccessor access)
        {
           _accessor = access;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = _accessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type==ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}