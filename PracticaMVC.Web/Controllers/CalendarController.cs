using Newtonsoft.Json;
using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.Web.Helpers;
using PracticaMVC.Web.Models;
using PracticaMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PracticaMVC.Web.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListDateCalendar(string MM, string YYYY)
        {
            var mes_ = int.Parse(MM);
            var anio_ = int.Parse(YYYY);

            var response = new FechasCalendario_BL().Get_DibujaCalendario(mes_, anio_);
            var jsonResponse = JsonConvert.SerializeObject(response.Data, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Json(jsonResponse);
        }

        public ActionResult Create(string Date)
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login", "Home");

            //Valida que sea un usuario
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var fechaSplit = Date.Split('-');
            var date_ = fechaSplit[0];
            var time_ = fechaSplit[1];
            DateTime dt = DateTime.ParseExact(date_ + " " + time_, "ddMMyyyy HHmmss", CultureInfo.InvariantCulture);

            Detalle_FechasCalendarioVM detalleVM = new Detalle_FechasCalendarioVM()
            {
                Usuarios = new Controles_DDL_BL().GetUsuarios_DDL("Seleccione").Data,
                Fecha = dt
            }; 

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["UsuariosCB"] = detalleVM.Usuarios; 
            //----------------------------------------------------------------------------------------------------

            return View(detalleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detalle_FechasCalendarioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            // Se mantienen los valores en la página en cada POST de los DropDownList
            viewModel.Usuarios = (IEnumerable<dynamic>)TempData["UsuariosCB"];
            //----------------------------------------------------------------------------------------------------

            if (ModelState.IsValid)
            {
                var Date_ = this.Request.QueryString["Date"];
                var datetime_ = DateTime.ParseExact(Date_, "ddMMyyyy", CultureInfo.InvariantCulture);
                viewModel.Fecha = datetime_;

                var dbResponse = new FechasCalendario_BL().UpsertFechasCalendario(
                               new FechasCalendario()
                               {
                                   IdFechaCalendario = viewModel.IdFechaCalendario,
                                   Fecha = viewModel.Fecha,
                                   IdUsuario = viewModel.IdUsuario,
                                   Asunto = viewModel.Asunto,
                                   Activo = viewModel.Activo
                               });
                if (!dbResponse.ExecutionOK || dbResponse.Data == null)
                {
                    this.ShowNotificacion("error", "Error", dbResponse.Message, "4", "0");
                    goto FIN;
                }
                TempData["MensajeAIndex"] = "Se ha actualizado correctamente";
                return RedirectToAction("Index");
            }
            else
                this.ShowNotificacion("error", "Error", "Verificar errores.", "4", "0");

            FIN:
            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["UsuariosCB"] = viewModel.Usuarios;
            //-----------------------------------------------------------------------------------

            return View(viewModel);
        }

        public ActionResult Details(Detalle_FechasCalendarioVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            // Se mantienen los valores en la página en cada POST de los DropDownList
            viewModel.Usuarios = (IEnumerable<dynamic>)TempData["UsuariosCB"];
            //----------------------------------------------------------------------------------------------------

            var id = this.Request.QueryString["Id"];
            viewModel.IdFechaCalendario = int.Parse(id);

            var dbResponse = new FechasCalendario_BL().GetInfoFechaCalendario(viewModel.IdFechaCalendario);
            if (!dbResponse.ExecutionOK || dbResponse.Data == null)
            {
                this.ShowNotificacion("error", "Error", dbResponse.Message, "4", "0");
                goto FIN;
            }

            viewModel.Asunto = dbResponse.Data.Asunto;
            viewModel.Fecha = (DateTime)dbResponse.Data.Fecha;
            viewModel.IdUsuario = (int)dbResponse.Data.IdUsuario;
            viewModel.Usuarios = new Controles_DDL_BL().GetUsuarios_DDL("Seleccione").Data;

            return View(viewModel);


        FIN:
            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["UsuariosCB"] = viewModel.Usuarios;
            //-----------------------------------------------------------------------------------

            return RedirectToAction("Index");
        }
    }
}