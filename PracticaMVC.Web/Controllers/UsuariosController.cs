using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.EN.Utils;
using PracticaMVC.Web.Helpers;
using PracticaMVC.Web.Models;
using PracticaMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PracticaMVC.Web.Controllers
{
    public class UsuariosController : Controller
    {
        /// <summary>
        /// Despliega el listado de usuarios
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = null;
            TempData["SortField"] = null;
            TempData["UsuarioFiltro"] = TempData["UsuarioFiltro"]?.ToString();
            TempData["tmpUsuariosPermisos"] = null;

            return View(GetUsuarios("", new Listado_UsuariosVM()));
        }

        /// <summary>
        /// Despliega el listado de usuarios para edicion
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexEdit()
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = null;
            TempData["SortField"] = null;
            TempData["UsuarioFiltro"] = TempData["UsuarioFiltro"]?.ToString();
            TempData["tmpUsuariosPermisos"] = null;

            return View(GetUsuarios("", new Listado_UsuariosVM()));
        }
        /// <summary>
        /// Despliega el listado de usuarios para graficar
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewGraphs()
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            return View(GetUsuarios("", new Listado_UsuariosVM()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexEdit(string control, Listado_UsuariosVM viewModel)
        {
            return View(GetUsuarios(control, viewModel));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_UsuariosVM viewModel)
        {
            return View(GetUsuarios(control, viewModel));
        }
        private Listado_UsuariosVM GetUsuarios(string control, Listado_UsuariosVM viewModel)
        {
            Listado_UsuariosVM listadoVM = new Listado_UsuariosVM();

            // Se mantienen los valores en la página en cada POST
            listadoVM.Orden = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : listadoVM.Orden;
            listadoVM.Campo = TempData["SortField"] != null ? TempData["SortField"].ToString() : listadoVM.Campo;
            listadoVM.Usuario = TempData["UsuarioFiltro"] != null ? TempData["UsuarioFiltro"].ToString() : listadoVM.Usuario;
            listadoVM.PaginaActual = viewModel.PaginaActual;

            //----------------------------------------------------------------------------------------------------

            switch (control)
            {
                case "Usuario":
                    if (listadoVM.Campo.Contains("Usuario"))
                    {
                        listadoVM.Orden = string.IsNullOrEmpty(listadoVM.Orden) ? " DESC" : "";
                    }
                    else
                    {
                        listadoVM.Campo = "Usuario";
                        listadoVM.Orden = "";
                    }
                    break;
                case "Password":
                    if (listadoVM.Campo.Contains("Password"))
                    {
                        listadoVM.Orden = string.IsNullOrEmpty(listadoVM.Orden) ? " DESC" : "";
                    }
                    else
                    {
                        listadoVM.Campo = "Password";
                        listadoVM.Orden = "";
                    }
                    break;
                case "Perfil":
                    if (listadoVM.Campo.Contains("Perfil"))
                    {
                        listadoVM.Orden = string.IsNullOrEmpty(listadoVM.Orden) ? " DESC" : "";
                    }
                    else
                    {
                        listadoVM.Campo = "IdPerfil";
                        listadoVM.Orden = "";
                    }
                    break;
                case "RolPerfil":
                    if (listadoVM.Campo.Contains("RolPerfil"))
                    {
                        listadoVM.Orden = string.IsNullOrEmpty(listadoVM.Orden) ? " DESC" : "";
                    }
                    else
                    {
                        listadoVM.Campo = "IdPerfil";
                        listadoVM.Orden = "";
                    }
                    break;
                case "EstatusRegistro":
                    if (listadoVM.Campo.Contains("EstatusRegistro"))
                    {
                        listadoVM.Orden = string.IsNullOrEmpty(listadoVM.Orden) ? " DESC" : "";
                    }
                    else
                    {
                        listadoVM.Campo = "IdPerfilRol";
                        listadoVM.Orden = "";
                    }
                    break;
                case "Buscar":
                    //Sólo se cambian el estos valores si se va a realizar una búsqueda
                    //No se deben cambiar al haber un POST por paginación u ordenamiento
                    listadoVM.PaginaActual = 1;
                    listadoVM.Usuario = viewModel.Usuario;
                    break;
            }

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["SortField"] = listadoVM.Campo;
            TempData["SortOrder"] = listadoVM.Orden;
            TempData["UsuarioFiltro"] = listadoVM.Usuario;
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<Usuarios>> response = new Usuarios_BL().GetUsuarios(listadoVM.Usuario,
                listadoVM.Campo + listadoVM.Orden, listadoVM.PaginaActual - 1, listadoVM.TotalFilas);
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
                            RolPerfil = u.PerfilesRoles.PerfilRol == null ? "" : u.PerfilesRoles.PerfilRol
                        });
                    }

                    listadoVM.UsuariosDDL = new Controles_DDL_BL().GetUsuarios_DDL("Seleccione").Data;
                    listadoVM.Listado = usuarios;
                    listadoVM.TotalPaginas = (int)Math.Ceiling(Convert.ToDecimal(response.NumRows) / Convert.ToDecimal(listadoVM.TotalFilas));

                    //TempData["tmpUsuarios"] = response.Data;
                }
            }
            else
            {
                listadoVM.TotalPaginas = 0;
            }

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return listadoVM;
        }

        public ActionResult Details(int? id)
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_UsuariosVM usuariosVM = new Detalle_UsuariosVM()
            {
                IdUsuario = id.Value
            };

            DBResponse<Usuarios> response = new Usuarios_BL().GetUsuarios_ById(id.Value);
            if (response.ExecutionOK)
            {
                usuariosVM.Usuario = response.Data.Usuario;
                usuariosVM.Password = response.Data.Password;
                usuariosVM.IdPerfil = response.Data.IdPerfil ?? 0;
                usuariosVM.IdPerfilRol = response.Data.IdPerfilRol ?? 0;

                usuariosVM.Perfiles = new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data;

                usuariosVM.PerfilesRoles = new Controles_DDL_BL().GetRolesPerfiles_DDL("Selecciona", usuariosVM.IdPerfil).Data;

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

            }
            else
            {
                this.ShowNotificacion("error", "Error", response.Message, "4", "0");
            }

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["PerfilesCB"] = usuariosVM.Perfiles;
            TempData["PerfilesRolesCB"] = usuariosVM.PerfilesRoles;
            TempData["tmpUsuariosPermisos"] = usuariosVM.Listado_Permisos;
            //----------------------------------------------------------------------------------------------------

            return View(usuariosVM);
        }

        /// <summary>
        /// Borra los filtros de la pantalla de lista
        /// </summary>
        /// <returns></returns>
        public ActionResult RestablecerFiltros()
        {
            TempData["SortField"] = null;
            TempData["SortOrder"] = null;
            TempData["UsuarioFiltro"] = null;

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Detalle_UsuariosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            // Se mantienen los valores en la página en cada POST de los DropDownList
            viewModel.Perfiles = (IEnumerable<dynamic>)TempData["PerfilesCB"];
            viewModel.PerfilesRoles = (IEnumerable<dynamic>)TempData["PerfilesRolesCB"];
            viewModel.Listado_Permisos = (List<Listado_Permisos>)TempData["tmpUsuariosPermisos"];
            //----------------------------------------------------------------------------------------------------

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["PerfilesCB"] = viewModel.Perfiles;
            TempData["PerfilesRolesCB"] = viewModel.PerfilesRoles;
            TempData["tmpUsuariosPermisos"] = viewModel.Listado_Permisos;
            //----------------------------------------------------------------------------------------------------

            if (!ModelState.IsValid)
            {
                this.ShowNotificacion("error", "Error", "Favor de completar los campos obligatorios.", "4", "0");
                return View(viewModel);
            }
            var response = new Usuarios_BL().GetUsuarios_ById(viewModel.IdUsuario).Data;

            var listPermisos = new List<Usuarios_Permisos>();
            foreach (var item in viewModel.Listado_Permisos)
            {
                Usuarios_Permisos permiso = new Usuarios_Permisos()
                {
                    IdPermiso = item.IdPermiso,
                    IdUsuarioPermiso = item.IdUsuarioPermiso,
                    Activo = true,
                    IdUsuario = viewModel.IdUsuario
                };

                listPermisos.Add(permiso);
            }

            DBResponse<Usuarios> dbResponse = new Usuarios_BL().UpsertUsuario(
                           new Usuarios()
                           {
                               IdUsuario = viewModel.IdUsuario,
                               Usuario = viewModel.Usuario,
                               Password = viewModel.Password,
                               IdPerfil = (int?)viewModel.IdPerfil,
                               IdEstatusRegistro = response.IdEstatusRegistro,
                               IdPerfilRol = viewModel.IdPerfilRol,
                               Activo = response.Activo,
                               Usuarios_Permisos = listPermisos
                           });
            if (!dbResponse.ExecutionOK || dbResponse.Data == null)
            {
                this.ShowNotificacion("error", "Error", dbResponse.Message, "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Se ha actualizado correctamente el usuario: " + viewModel.Usuario;
            return RedirectToAction("Index");
        }


        public ActionResult Create()
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_UsuariosVM detalleVM = new Detalle_UsuariosVM()
            {
                Perfiles = new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data
            };
            detalleVM.PerfilesRoles = new Controles_DDL_BL().GetRolesPerfiles_DDL("Selecciona", detalleVM.IdPerfil).Data;

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["PerfilesCB"] = detalleVM.Perfiles;
            TempData["PerfilesRolesCB"] = detalleVM.PerfilesRoles;
            //----------------------------------------------------------------------------------------------------

            return View(detalleVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detalle_UsuariosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            // Se mantienen los valores en la página en cada POST de los DropDownList
            viewModel.Perfiles = (IEnumerable<dynamic>)TempData["PerfilesCB"];
            viewModel.PerfilesRoles = (IEnumerable<dynamic>)TempData["PerfilesRolesCB"];
            //----------------------------------------------------------------------------------------------------

            if (ModelState.IsValid)
            {
                DBResponse<Usuarios> dbResponse = new Usuarios_BL().UpsertUsuario(
                               new Usuarios()
                               {
                                   IdUsuario = viewModel.IdUsuario,
                                   Usuario = viewModel.Usuario,
                                   Password = viewModel.Password,
                                   IdPerfil = (int?)viewModel.IdPerfil,
                                   IdPerfilRol = (int?)viewModel.IdPerfilRol
                               });
                if (!dbResponse.ExecutionOK || dbResponse.Data == null)
                {
                    this.ShowNotificacion("error", "Error", dbResponse.Message, "4", "0");
                    goto FIN;
                }
                TempData["MensajeAIndex"] = "Se ha actualizado correctamente el usuario: " + viewModel.Usuario;
                return RedirectToAction("Index");
            }
            else
                this.ShowNotificacion("error", "Error", "Verificar errores.", "4", "0");

            FIN:
            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["PerfilesCB"] = viewModel.Perfiles;
            TempData["PerfilesRolesCB"] = viewModel.PerfilesRoles;
            //-----------------------------------------------------------------------------------

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstatusUpdate(int? id)
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var response = new Usuarios_BL().InactivarEliminarUsuario(id.Value);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }

        public FileResult ExportarExcel(string usuario)
        {

            List<Usuarios> usuariosList = new Usuarios_BL().GetUsuarios_List(usuario).Data;

            DBResponse<byte[]> response = new Generics_BL().GrabaArchivoExcelSimple(GetData(usuariosList), "", "ListadoUsuarios", ConfigurationManager.AppSettings["DirectorioPlantillas"].ToString());
            if (!response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;

            return File(response.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListadoUsuarios.xls");
        }

        private DataTable GetData(List<Usuarios> _listado)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Password");
            dt.Columns.Add("Perfil");
            dt.Columns.Add("Rol del usuario");
            dt.Columns.Add("Estatus Registro");


            DataRow dr;

            foreach (var row in _listado)
            {
                dr = dt.NewRow();
                dr[0] = row.Usuario;
                dr[1] = row.Password;
                dr[2] = row.Perfiles.Perfil;
                dr[3] = row.PerfilesRoles.PerfilRol;
                dr[4] = row.EstatusRegistros.EstatusRegistro;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Inserta el permiso de usuario
        /// </summary>
        /// <param name="usuariosVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertPermiso(Detalle_UsuariosPermisosVM usuariosVM)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            usuariosVM.Listado_Permisos = (List<Listado_Permisos>)TempData["tmpUsuariosPermisos"];

            if (usuariosVM.Listado_Permisos.Any(x => x.IdPermiso == usuariosVM.IdPermiso))
            {
                this.ShowNotificacion("success", "Información", "El Permiso que desea agregar, ya existe para el usuario", "4", "0");
            }
            else
            {
                usuariosVM.Listado_Permisos.Add(new Listado_Permisos()
                {
                    IdPermiso = usuariosVM.IdPermiso,
                    IdUsuarioPermiso = 0,
                    Permiso = new Usuarios_BL().GetPermiso_ById(usuariosVM.IdPermiso).Data.Permiso
                });

            }

            TempData["tmpUsuariosPermisos"] = usuariosVM.Listado_Permisos;

            return PartialView("ModalPermisos", usuariosVM.Listado_Permisos);
        }

        /// <summary>
        /// Elimina el permiso de usuario
        /// </summary>
        /// <param name="usuariosVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePermisoUsuario(int IdPermisoUsuario)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var listadoPermisos = (List<Listado_Permisos>)TempData["tmpUsuariosPermisos"];

            if (listadoPermisos.Any(x => x.IdPermiso == IdPermisoUsuario))
            {
                int indexRow = listadoPermisos.FindIndex(x => x.IdPermiso == IdPermisoUsuario);
                listadoPermisos.RemoveAt(indexRow);
            }

            TempData["tmpUsuariosPermisos"] = listadoPermisos;

            return PartialView("ModalPermisos", listadoPermisos);
        }

        /// <summary>
        /// Devuelve un listado de roles del perfil seleccionado
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRolesByPerfil(int idPerfil)
        {
            var artistas = new Controles_DDL_BL().GetRolesPerfiles_DDL("Selecciona", idPerfil).Data;
            SelectList items = new SelectList(
                artistas,
                "Valor", "Texto", 0);
            TempData["PerfilesRolesCB"] = artistas;
            return Json(artistas);
        }

        /// <summary>
        /// Devuelve un listado de los perfiles
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPerfiles_DDL(string selectedValue)
        {
            IDictionary<string, string> perfilesUsuario = new Dictionary<string, string>();

            var perfiles_ = new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data;
            SelectList itemList = new SelectList(
                perfiles_,
                "Valor", "Texto", selectedValue);

            foreach (var item in itemList)
            {
                perfilesUsuario.Add(item.Value, item.Text);
            }

            var valueSelected = itemList.Where(x => x.Selected == true).ToList().FirstOrDefault().Text;

            perfilesUsuario.Add("selected", valueSelected);


            var perfilesJson = Json(perfilesUsuario);
            return Json(perfilesUsuario);
        }

        [HttpPost]
        public ActionResult UploadCargaMasiva(HttpPostedFileBase fileData)
        {
            string path = Server.MapPath("~/Archivos/");
            string fileName = "";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (fileData != null)
            {
                if (Path.GetExtension(fileData.FileName).ToLower() != ".xlsx")
                    return Json("Error");

                fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + Path.GetFileName(fileData.FileName);
                fileData.SaveAs(path + fileName);

                var CargaMasivaInsert = new Generics_BL().CargaMasivaUsuarios(path + fileName);

            }

            return Json(fileName);
        }

        [HttpPost]
        public ActionResult SaveChanges(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            int? intNulleable = null;
            if (!ModelState.IsValid)
            {
                var responseImvalid_ = new { value = value, status = status, message = "Favor de completar los campos obligatorios." };
                JObject oInvalid = JObject.FromObject(responseImvalid_);
                return Content(oInvalid.ToString());
            }
            var response = new Usuarios_BL().GetUsuarios_ById(id).Data;
            response.Usuario = propertyName == "Usuario" ? value : response.Usuario;
            response.Password = propertyName == "Password" ? value : response.Password;
            response.IdPerfil = propertyName == "Perfil" ? (value == "0" ? intNulleable : int.Parse(value)) : response.IdPerfil;

            var listPermisos = new List<Usuarios_Permisos>();
            foreach (var item in response.Usuarios_Permisos)
            {
                Usuarios_Permisos permiso = new Usuarios_Permisos()
                {
                    IdPermiso = item.IdPermiso,
                    IdUsuarioPermiso = item.IdUsuarioPermiso,
                    Activo = true,
                    IdUsuario = item.IdUsuario
                };

                listPermisos.Add(permiso);
            }

            if (propertyName == "Perfil")
            {
                var perfiles_ = new Controles_DDL_BL().GetPerfiles_DDL("Selecciona").Data;
                SelectList items = new SelectList(
                perfiles_,
                "Valor", "Texto", value);

                value = items.Where(x => x.Selected == true).ToList().FirstOrDefault().Text;
            }

            DBResponse<Usuarios> dbResponse = new Usuarios_BL().UpsertUsuario(
                           new Usuarios()
                           {
                               IdUsuario = response.IdUsuario,
                               Usuario = response.Usuario,
                               Password = response.Password,
                               IdPerfil = (int?)response.IdPerfil,
                               IdEstatusRegistro = response.IdEstatusRegistro,
                               IdPerfilRol = response.IdPerfilRol,
                               Activo = response.Activo,
                               Usuarios_Permisos = listPermisos
                           });
            if (!dbResponse.ExecutionOK || dbResponse.Data == null)
            {
                var responseNotOk_ = new { value = value, status = dbResponse.ExecutionOK, message = dbResponse.Message };
                return Content(responseNotOk_.ToString());
            }

            var response_ = new { value = value, status = dbResponse.ExecutionOK, message = message };
            JObject o = JObject.FromObject(response_);
            return Content(o.ToString());

        }

        [HttpPost]
        public JsonResult ShowGraphic(string type, string title, string tooltip)
        {
            List<dataGrafica> datosGrafica = new List<dataGrafica>();
            DBResponse<List<Usuarios>> dataResponseUsuarios = new Usuarios_BL().GetUsuarios_List("");
            if (dataResponseUsuarios.ExecutionOK)
            {
                var dataUsuarios = dataResponseUsuarios.Data;
                foreach (var itemUser in dataUsuarios)
                {
                    var visitados = itemUser.Usuarios_Visitados.Sum(x => x.NumVisitas);
                    dataGrafica dataGraficas = new dataGrafica()
                    {
                        name = itemUser.Usuario,
                        value = visitados == null ? 0 : (int)visitados
                    };

                    datosGrafica.Add(dataGraficas);
                }
            }

            var execGraphic = (new BL.Utils.GeneraGraficas()).GeneraJsonGrafica(title, datosGrafica, type, tooltip, true);            

            return Json(JsonConvert.SerializeObject(execGraphic, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}