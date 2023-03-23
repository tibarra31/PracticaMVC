using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticaMVC.Web.ViewModels
{
    public class Detalle_FechasCalendarioVM
    {
        [Key]
        [Display(Name = "ID")]
        public int IdFechaCalendario { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Asunto")]
        [Required(ErrorMessage = "Este dato es requerido")]
        public string Asunto { get; set; }
        public int IdUsuario { get; set; }
        public IEnumerable<dynamic> Usuarios { get; set; }
        public Boolean Activo { get; set; }

    }
}