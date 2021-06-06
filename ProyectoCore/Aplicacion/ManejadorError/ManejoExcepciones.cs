using System;
using System.Net;

namespace Aplicacion.ManejadorError
{
    public class ManejoExcepciones: Exception
    {
        public HttpStatusCode Codigo {get;}
        public object Errors {get;}
        public ManejoExcepciones(HttpStatusCode status, object error = null)
        {
           Codigo = status;
           Errors = error;
           
        }
    }
}