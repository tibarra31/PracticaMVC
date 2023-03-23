using PracticaMVC.API.Models;
using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.EN.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace PracticaMVC.API.Controllers
{
    public class NotificacionesController : ApiController
    {
        Notificaciones_BL notificaciones = new Notificaciones_BL();

        [HttpPost]
        [ResponseType(typeof(List<Notificaciones_Usuarios>))]
        [Route("api/GetNotificaciones/{idUsuario}")]
        public async Task<IHttpActionResult> GetNotificaciones(int idUsuario)
        {
            var response = notificaciones.GetListNotificaUsuarioRecibe(idUsuario);

            return Ok(response);
        }
    }
}