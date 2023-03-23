using PracticaMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.ViewModels
{
    public class Detalle_UsuariosPermisosVM
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public int IdPermiso { get; set; }

        [Display(Name = "Permiso")]
        public string Permiso { get; set; }
        
        public IEnumerable<dynamic> PermisosUsuarios { get; set; }

        public List<Listado_Permisos> Listado_Permisos { get; set; }

    }
}