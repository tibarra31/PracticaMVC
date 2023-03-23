using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.Models
{
    public class Listado_Permisos
    {
        public int IdUsuarioPermiso { get; set; }

        [Display(Name = "ID")]
        public int IdPermiso { get; set; }

        [Display(Name = "Permiso")]
        public string Permiso { get; set; }
    }
}