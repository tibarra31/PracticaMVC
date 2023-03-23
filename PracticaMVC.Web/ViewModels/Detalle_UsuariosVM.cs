using PracticaMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.ViewModels
{
    public class Detalle_UsuariosVM
    {
        [Key]
        [Display(Name = "ID")]
        public int IdUsuario { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public string Usuario { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public string Password { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public int IdPerfil { get; set; }
        public IEnumerable<dynamic> Perfiles { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public int IdPerfilRol { get; set; }
        public IEnumerable<dynamic> PerfilesRoles { get; set; }

        [Display(Name = "Permisos")]

        public List<Listado_Permisos> Listado_Permisos { get; set; } = new List<Listado_Permisos>();

        public Detalle_UsuariosPermisosVM permisosUsuario { get; set; } = new Detalle_UsuariosPermisosVM();

        public int IdPermisoUsuario { get; set; }

    }
}