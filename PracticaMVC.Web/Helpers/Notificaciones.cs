using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticaMVC.Web.Helpers
{
    public static class Notificaciones
    {
        public static void ShowNotificacion(this Controller controller, string tipo, string titulo,
            string mensaje, string segundosVisible, string segundosRetardo)
        {
            if (controller.TempData.ContainsKey("messages"))
            {
                (controller.TempData["messages"] as Dictionary<string, string[]>).Add(
                    tipo,
                    new string[]
                    {
                        titulo,
                        mensaje,
                        segundosVisible,
                        segundosRetardo
                    });
            }
            else
            {
                controller.TempData["messages"] = new Dictionary<string, string[]>
                {
                    [tipo] = new string[]
                                {
                                    titulo,
                                    mensaje,
                                    segundosVisible,
                                    segundosRetardo
                                }
                };
            }
        }
    }
}