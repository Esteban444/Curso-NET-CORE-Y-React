using System.Collections.Generic;
using Dominio;

namespace Aplicacion.Contratos
{
    public interface IjwtGenerador
    {
         string CrearToken(Usuario usuario, List<string> roles); // el segundo parametro fue agregado para  los roles
    }
}