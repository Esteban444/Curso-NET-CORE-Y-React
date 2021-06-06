using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController: MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> Get()
        {
             return await Mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetById(Guid id)
        {
           return await Mediator.Send(new ConsultaId.CursoUnico{Id = id});
        } 

        [HttpPost]
        public async Task<ActionResult<Unit>> CrearCurso(NuevoCurso.Ejecutar data) 
        {
           return await Mediator.Send(data);
        }   

        [HttpPut("{id}")]
         public async Task<ActionResult<Unit>> EditarCurso(Guid id,EditarCurso.EjecutarE data)  
        {
            data.CursoId = id;
           return await Mediator.Send(data);
        }    

         [HttpDelete("{id}")]
         public async Task<ActionResult<Unit>> EliminarCurso(Guid id) 
        {
              return await Mediator.Send(new EliminarCurso.Ejecutar{Id = id});
        }

        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>>  Report(PaginacionCurso.EjecutaPaginacion data)      
        {
             return await Mediator.Send(data);
        } 
    }
}