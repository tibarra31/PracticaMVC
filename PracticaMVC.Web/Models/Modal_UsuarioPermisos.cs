using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.Models
{
    public class Modal_UsuarioPermisos
    {
        [Key]
        [Display(Name = "ID")]
        public int IdUsuarioPermiso { get; set; }
        
        public int IdUsuario { get; set; }

        public int IdPermiso { get; set; }

        public string Permiso { get; set; }

        public bool Activo { get; set; } = false;

        public IEnumerable<dynamic> PermisosUsuarios { get; set; }

        public List<Listado_Permisos> Listado_Permisos { get; set; } = new List<Listado_Permisos>();
    }
}