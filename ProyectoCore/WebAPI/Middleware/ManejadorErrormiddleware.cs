using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrormiddleware
    {
        private readonly RequestDelegate _nextRequest;
        private readonly ILogger<ManejadorErrormiddleware> _logger;
        public ManejadorErrormiddleware(RequestDelegate next,ILogger<ManejadorErrormiddleware> logger)
        {
           _nextRequest = next;
           _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try{
           await _nextRequest(context);
            }catch(Exception ex)
            {
               await ManejadorExcepcionAsincrono(context, ex, _logger);
            }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex,ILogger<ManejadorErrormiddleware> logger)
        {
           object errores = null;
           switch(ex)
           {
                case ManejoExcepciones me :
                       logger.LogError(ex, "Manejo de errores");
                       errores = me.Errors;
                       context.Response.StatusCode =(int)me.Codigo;
                break;
                case Exception e:
                       logger.LogError(ex, "Error de servidor");
                       errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                       context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;

           }
           context.Response.ContentType = "application/json";
           if(errores != null)
           {
               var resultados = JsonConvert.SerializeObject(new {errores});
               await context.Response.WriteAsync(resultados);
           }
        }
    }
}