using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.Web.Models;
using PracticaMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticaMVC.Web.Controllers
{
    public class UsuariosAngularController : Controller
    {
        // GET: UsuariosAngular
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detalle()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetUsuariosAngular(Filtros_Usuarios objFiltros)
        {
            var dbResponse = new DBResponse<List<Listado_Usuarios>>();
            DBResponse<List<Usuarios>> response = new Usuarios_BL().GetUsuariosAngular(objFiltros);
            if (response.ExecutionOK)
            {
                List<Listado_Usuarios> usuarios = new List<Listado_Usuarios>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (Usuarios u in response.Data)
                    {
                        usuarios.Add(new Listado_Usuarios()
                        {
                            IdUsuario = u.IdUsuario,
                            Usuario = u.Usuario,
                            IdPerfil = u.IdPerfil == null ? 0 : (int)u.IdPerfil,
                            Perfil = u.Perfiles == null ? "" : u.Perfiles.Perfil,
                            IdEstatusRegistro = (int)u.IdEstatusRegistro,
                            EstatusRegistro = u.EstatusRegistros == null ? "" : u.EstatusRegistros.EstatusRegistro,
                            Password = u.Password,
                            RolPerfil = u.PerfilesRoles.PerfilRol ?? ""
                        });
                    }
                    dbResponse.Data = usuarios;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "ok";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = new List<Listado_Usuarios>();
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Sin Información";
                    return Json(dbResponse);
                }
            }
            else
            {
                dbResponse.Data = new List<Listado_Usuarios>();
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Sin Información";
                return Json(dbResponse);
            } 
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Detalle_UsuariosVM ObjUsuarios)
        {
            var dbResponse = new DBResponse<int>();

            var response = new Usuarios_BL().GetUsuarios_ById(ObjUsuarios.IdUsuario).Data;

            var listPermisos = new List<Usuarios_Permisos>();
            foreach (var item in ObjUsuarios.Listado_Permisos)
            {
                Usuarios_Permisos permiso = new Usuarios_Permisos()
                {
                    IdPermiso = item.IdPermiso,
                    IdUsuarioPermiso = item.IdUsuarioPermiso,
                    Activo = true,
                    IdUsuario = ObjUsuarios.IdUsuario
                };

                listPermisos.Add(permiso);
            }

            DBResponse<Usuarios> dbResponseUpsert = new Usuarios_BL().UpsertUsuario(
                           new Usuarios()
                           {
                               IdUsuario = ObjUsuarios.IdUsuario,
                               Usuario = ObjUsuarios.Usuario,
                               Password = ObjUsuarios.Password,
                               IdPerfil = (int?)ObjUsuarios.IdPerfil,
                               IdEstatusRegistro = 1,
                               IdPerfilRol = ObjUsuarios.IdPerfilRol,
                               Activo = true,
                               Usuarios_Permisos = listPermisos
                           });
            if (!dbResponseUpsert.ExecutionOK)
            {
                dbResponse.Data = 0;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = dbResponseUpsert.Message; 
            }
            else
            {
                dbResponse.Data = dbResponseUpsert.Data.IdUsuario;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "";
            }

            return Json(dbResponse);
        }

        [HttpPost]
        public JsonResult GetUsuariosAngularByIdUsuario(int IdUsuario)
        {
            var dbResponse = new DBResponse<Detalle_UsuariosVM>();
            var response = new Usuarios_BL().GetUsuarios_ById(IdUsuario);
            if (response.ExecutionOK)
            {
                List<Listado_Usuarios> usuarios = new List<Listado_Usuarios>();
                if (response.Data != null)
                {
                    Detalle_UsuariosVM usuariosVM = new Detalle_UsuariosVM()
                    {
                        IdUsuario = IdUsuario,
                        Usuario = response.Data.Usuario,
                        Password = response.Data.Password,
                        IdPerfil = response.Data.IdPerfil ?? 0,
                        IdPerfilRol = response.Data.IdPerfilRol ?? 0,
                        Perfiles = new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data,
                        PerfilesRoles = new Controles_DDL_BL().GetRolesPerfiles_DDL("Selecciona", (int)response.Data.IdPerfil).Data
                    };
                    usuariosVM.permisosUsuario.PermisosUsuarios = new Controles_DDL_BL().GetPermisos_DDL("Selecciona").Data;

                    foreach (var item in response.Data.Usuarios_Permisos)
                    {
                        Listado_Permisos listado = new Listado_Permisos()
                        {
                            IdPermiso = (int)item.IdPermiso,
                            IdUsuarioPermiso = item.IdUsuarioPermiso,
                            Permiso = item.Permisos.Permiso,
                        };
                        usuariosVM.Listado_Permisos.Add(listado);
                    }
                    dbResponse.Data = usuariosVM;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "ok";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = new Detalle_UsuariosVM();
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Sin Información";
                    return Json(dbResponse);
                }
            }
            else
            {
                dbResponse.Data = new Detalle_UsuariosVM();
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Sin Información";
                return Json(dbResponse);
            }
        }
        [HttpPost]
        public JsonResult GetComboPerfiles()
        {
            return Json(new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data);
        }

        [HttpPost]
        public JsonResult DeleteUsuario(int IdUsuario)
        {

            var dbResponse = new DBResponse<int>();
            var response = new Usuarios_BL().InactivarEliminarUsuario(IdUsuario);
            if (response.ExecutionOK)
            {
                if (response.Data != null)
                {
                    dbResponse.Data = 1;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "ok";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = 0;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al eliminar el registro " + response.Message;
                    return Json(dbResponse);
                }
            }
            else
            {
                dbResponse.Data = 0;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Ocurrio un error al eliminar el registro " + response.Message;
                return Json(dbResponse);
            }
        }
        [HttpPost]
        public JsonResult GetComboRolesPerfiles(int IdPerfil)
        {
            return Json(new Controles_DDL_BL().GetRolesPerfiles_DDL("Selecciona", IdPerfil).Data);
        }
        [HttpPost]
        public JsonResult GetComboPermisos()
        {
            return Json(new Controles_DDL_BL().GetPermisos_DDL("Selecciona").Data);
        }
    }
}