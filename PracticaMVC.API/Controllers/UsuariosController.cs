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
    public class UsuariosController : ApiController
    {
        Usuarios_BL usuariosBL = new Usuarios_BL();

        [HttpPost]
        [ResponseType(typeof(Usuarios))]
        [Route("api/Login")]
        public async Task<IHttpActionResult> Login(Usuarios login)
        {
            var response = usuariosBL.ValidaUsuarioLogin(login.Usuario, login.Password);
            if (response.ExecutionOK)
            {
                var getNotificaciones = (new Notificaciones_BL()).GetListNotificaUsuarioRecibe(response.Data.IdUsuario);
            }
            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(DBResponse<int>))]
        [Route("api/RegisterUser")]
        public async Task<IHttpActionResult> RegistrarUsuario(RegistroUsuarios user)
        {
            DBResponse<int> response = new DBResponse<int>();
            Usuarios usuario = new Usuarios
            {
                IdUsuario = user.IdUsuario,
                Usuario = user.Usuario,
                Password = user.Password,
                IdPerfil = user.IdPerfil,
                Activo = user.Activo,
                IdEstatusRegistro = user.IdEstatusRegistro,
                Archivo = user.Archivo,
                ImagenUsuario = user.Archivo.Nombre
            };
            string msj = "";

            if (usuariosBL.ValidacionesUsuarios(out msj, usuario))
            {
                var responseUpsert = usuariosBL.UpsertUsuario(usuario);
                if (responseUpsert.ExecutionOK)
                {
                    response.ExecutionOK = true;
                    response.Message = responseUpsert.Message.Replace("<br>", "\n");

                    var doc = usuario.Archivo;
                    var stream = new MemoryStream(doc.DocArray);
                    var file = doc.Nombre;
                    var folder = $"/ArchivosUsuarios";
                    var fullPath = $"{folder}/{file}";
                    var upload = FilesHelper.UplodDocument(stream, folder, file);
                }
            }
            else
            {
                response.ExecutionOK = false;
                response.Message = msj.Replace("<br>", "\n");
            }

            return Ok(response);
        }

        [HttpPost]
        [ResponseType(typeof(DBResponse<List<Usuarios>>))]
        [Route("api/GetUsuarios")]
        public async Task<IHttpActionResult> Get_ListEmpleados(FiltroUsuario filtros)
        {
            DBResponse<List<Usuarios>> response = usuariosBL.GetUsuarios_List(filtros.NombreUsuario);
            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(DBResponse<Usuarios>))]
        [Route("api/GetUser/{idUsuario}")]
        public async Task<IHttpActionResult> GetUsuario(int idUsuario)
        {
            var response = usuariosBL.GetUsuarios_ById(idUsuario);

            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(DBResponse<List<ControlDDL>>))]
        [Route("api/GetPerfilesDDL")]
        public async Task<IHttpActionResult> GetPerfilesDDL()
        {
            DBResponse<List<ControlDDL>> response = new Controles_DDL_BL().GetPerfilesApp_DDL("Seleccione");
            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(DBResponse<Usuarios>))]
        [Route("api/InactiveUser")]
        public async Task<IHttpActionResult> InactiveUser(Usuarios user)
        {
            var response = usuariosBL.GetUsuarios_ById(user.IdUsuario);
            if (response.ExecutionOK)
            {
                response.Data.Activo = false;
                var responseUpsert = usuariosBL.UpsertUsuario(response.Data);
                if (responseUpsert.ExecutionOK)
                {
                    response.ExecutionOK = true;
                    response.Message = responseUpsert.Message.Replace("<br>", "\n");
                }
            }
            return Ok(response);
        }

    }
}