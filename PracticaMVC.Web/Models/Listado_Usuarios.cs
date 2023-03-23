using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.Models
{
    public class Listado_Usuarios
    {
        [Key]
        [Display(Name = "ID")]
        public int IdUsuario { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "IdPerfil")]
        public int IdPerfil { get; set; }

        [Display(Name = "Perfil")]
        public string Perfil { get; set; }

        [Display(Name = "IdEstatusRegistro")]
        public int IdEstatusRegistro { get; set; }

        [Display(Name = "EstatusRegistro")]
        public string EstatusRegistro { get; set; }

        [Display(Name = "RolPerfil")]
        public string RolPerfil { get; set; }
    }
}