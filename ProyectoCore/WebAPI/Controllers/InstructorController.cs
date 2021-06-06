using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;

namespace WebAPI.Controllers
{
    public class InstructorController: MiControllerBase
    {
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructor()
        {
                return await Mediator.Send(new Consulta.Lista());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
           return await Mediator.Send(new ConsultaId.Ejecuta{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CrearInstructor(NuevoInstructor.EjecutaI data)
        {
           return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> ActualizarInstructor(Guid id,EditarInstructor.EjecutaIE data)
        {
          data.InstructorId = id;
          return await Mediator.Send(data);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<Unit>> EliminarInstructor(Guid id)
        {
            return await Mediator.Send(new EliminarInstructor.EjecutaE{Id = id});
        }
    }
}