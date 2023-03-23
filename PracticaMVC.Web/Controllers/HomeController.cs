using PracticaMVC.BL;
using PracticaMVC.EN;
using PracticaMVC.Web.Helpers;
using PracticaMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticaMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserLogin"] == null)
                return RedirectToAction("Login");

            //Valida que sea un usuario 
            if (!(Session["UserLogin"] is Usuarios cc))
                return RedirectToAction("Login", "Home");

            return View();
        }

        public ActionResult Login()
        {
            Session["UserLogin"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(InicioSesionVM inicioSesion)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            if (ModelState.IsValid)
            {
                DBResponse<Usuarios> obj = new Usuarios_BL().ValidaUsuarioLogin(inicioSesion.Usuario, inicioSesion.Contrasena);
                if (obj.ExecutionOK && obj.Data != null)
                {
                    Session["UserLogin"] = obj.Data;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }              

                this.ShowNotificacion("error", "Error", "Usuario o contraseña incorrectos.", "0", "0");
            }

            return View(inicioSesion);
        }

        /// <summary>
        /// Abre la pantalla de Error con el mensaje que le indican
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            ErrorVM errorVM = new ErrorVM
            {
                MensajeError = TempData["MensajeError"].ToString()
            };

            TempData["MensajeError"] = errorVM.MensajeError;
            return View(errorVM);
        }


        /// <summary>
        /// Despliega una pantalla con un mensaje al usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Informacion()
        {
            InformacionVM informacionVM = new InformacionVM
            {
                Mensaje = TempData["MensajeInformacion"].ToString()
            };

            TempData["MensajeInformacion"] = informacionVM.Mensaje;
            return View(informacionVM);
        }
    }
}