using PracticaMVC.EN;
using PracticaMVC.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticaMVC.Web.ViewModels
{
    public class Listado_CalendarioVM
    {
        #region Filtros especiales de la página
        public int IdUsuario { get; set; }
        public string Fecha { get; set; }
        #endregion

        public List<FechasCalendario> FechasCalendario { get; set; }
    }
}