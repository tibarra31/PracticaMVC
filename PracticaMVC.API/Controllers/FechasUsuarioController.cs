using PracticaMVC.API.Models;
using PracticaMVC.API.Utils;
using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace PracticaMVC.API.Controllers
{
    public class FechasUsuarioController : ApiController
    {
        FechasCalendario_BL fechasCalendarioBL = new FechasCalendario_BL();

        [HttpPost]
        [ResponseType(typeof(DBResponse<List<FechasCalendario>>))]
        [Route("api/GetUsuarioCalendario")]
        public async Task<IHttpActionResult> GetUsuarioCalendario(FiltroUsuario filtros)
        {
            DBResponse<List<FechasCalendario>> response = fechasCalendarioBL.Get_UsuariosCalendario(filtros.IdUsuario);

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(DBResponse<FechasCalendario>))]
        [Route("api/RegisterDatesUser")]
        public async Task<IHttpActionResult> RegisterDatesUser(FechasCalendario fechaCalendario)
        {
            DBResponse<FechasCalendario> response = fechasCalendarioBL.UpsertFechasCalendario(fechaCalendario);
            return Ok(response);
        }
    }
}